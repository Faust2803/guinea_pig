using System.Collections.Generic;
using DataModels.Achievement;
using DataModels.PlayerData;
using Managers;
using Managers.Analytics;
using Managers.HamsterPreviewManager;
using UI.Panels;
using UI.Windows.CollectionWindow.CollectionItems;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using Zenject;

namespace UI.Windows.CollectionWindow
{
    public class CollectionWindowMediator :BaseWindowMediator<CollectionWindowView, WindowData>
    {
        [Inject] private PlayerManager _playerManager;
        [Inject] private HamsterPreviewManager _hamsterPreviewManager;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private TutorialManager _tutorialManager;
        [Inject] private AchievementManager _achievementManager;
        

        private CollectionItemView _collectionItemView = null;

        protected override void ShowStart()
        {
            base.ShowStart();

            _uiManager.ClosePanel(PanelType.TamagochiPanel);

            _hamsterPreviewManager.ShowPreview(true);

            _playerManager.Resources[ResourcesType.GoldenBean].SetNewValue += UpdateResource;
            UpdateResource(
                _playerManager.Resources[ResourcesType.GoldenBean].Value,
                _playerManager.Resources[ResourcesType.GoldenBean].ResourceType);

            Target.BuyButton.onClick.AddListener(async () =>
            {
                var windowData = new SimpleDialogWindowData
                {
                    Title = "Great Choice!",
                    Description = $"Are You ready to spend {_collectionItemView.CollectionItemDataModel.collection_cost} coins?",
                    YesButtonText = "Yes",
                    YesAction = async () =>
                    {
                        var result =await _playerManager.BuyCollectionItemInMarket(_collectionItemView.CollectionItemDataModel.collection_id);

                        if (result == null) return;

                        await _playerManager.EquipCollectionItem(_collectionItemView.CollectionItemDataModel.collection_id);

                        Target.UpdateWindowView(_playerManager.PlayerCollection);
                        Target.UpdateWindowView(_collectionItemView.CollectionItemDataModel);
                        
                        var achievementTargets = new List<AchievementTarget>();
                        achievementTargets.Add(new AchievementTarget(TargetType.BuyHamster, 1));
                        achievementTargets.Add(new AchievementTarget(TargetType.CollectHamsters, 1));
                        achievementTargets.Add(new AchievementTarget(TargetType.SpendBeans, result.collection_cost));
                        _analyticsManager.CollectionBuyHamsta(_collectionItemView.CollectionItemDataModel
                            .collection_name);
                        await _achievementManager.CheckAchievementTarget(achievementTargets.ToArray());
                    }
                };

                _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
            });

            Target.EquipButton.onClick.AddListener(async () =>
            {
                await _playerManager.EquipCollectionItem(_collectionItemView.CollectionItemDataModel.collection_id);

                Target.UpdateWindowView(_playerManager.PlayerCollection);
                Target.UpdateWindowView(_collectionItemView.CollectionItemDataModel);
                _analyticsManager.CollectionEquipmentHamsta();
            });

            foreach (var t in _playerManager.PlayerCollection)
            {
                var collectionItem = Target.InstantiateCollectioItem();
                collectionItem.UpdateItemView(t.Value);

                collectionItem.UnlockedButton.onClick.AddListener(() =>
                {
                    if (_collectionItemView != null)
                        _collectionItemView.SetDefaultState();

                    _collectionItemView = collectionItem;
                    _collectionItemView.SetSelectedState();

                    Target.UpdateWindowView(t.Value);

                    _hamsterPreviewManager.UpdatedHamsterPreview(t.Value);
                });
            }

            Target.HamsterPreviewView.UpdatePreview(_playerManager.CurrentCollectionItem);

            var poz = Target.CollectionItemTransform.position;
            Target.CollectionItemTransform.position = new Vector3(poz.x, 0, poz.z);
        }

        protected override void ShowEnd()
        {
            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.FirstEnterCollection))
            {
                var windowData = new SimpleDialogWindowData
                {
                    Title = "Collect them all!",
                    Description = "Collect All the Hamstas to join the AIRDROP!",
                    NoButtonActive = false,
                    YesAction = async () =>
                    {
                        _tutorialManager.SaveTutorialSteps(TutorialSteps.FirstEnterCollection);
                    },
                    YesButtonText = "Yep!"

                };
                _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
            }
        }


        protected override void CloseStart()
        { 
            base.CloseStart();

            _hamsterPreviewManager.ShowPreview(false);

            _hamsterPreviewManager.UpdatedHamsterPreview();

            _uiManager.OpenPanel(PanelType.TamagochiPanel);
        }

        private void UpdateResource(int value, ResourcesType type)
        {
            Target.CurrencyItem.SetCurrencyData(value, type);
        }
    }
}