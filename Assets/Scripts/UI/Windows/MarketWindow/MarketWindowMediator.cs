using DataModels.PlayerData;
using Managers;
using System;
using System.Collections.Generic;
using DataModels.Achievement;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using Zenject;

namespace UI.Windows.MarketWindow
{
    public class MarketWindowMediator :BaseWindowMediator<MarketWindowView, WindowData>
    {
        [Inject] private PlayerManager _playerManager;
        [Inject] private AchievementManager _achievementManager;

        protected override void ShowStart()
        {
            base.ShowStart();

            _playerManager.Resources[ResourcesType.GoldenBean].SetNewValue += UpdateResource;
            UpdateResource(
                _playerManager.Resources[ResourcesType.GoldenBean].Value,
                _playerManager.Resources[ResourcesType.GoldenBean].ResourceType);

            foreach (var t in _playerManager.MarketCollection)
            {
                var collectionItem = Target.InstantiateMarketItem();
                collectionItem.UpdateItemView(t.Value);

                collectionItem.BuyButton.onClick.AddListener(() =>
                {
                    var windowData = new SimpleDialogWindowData
                    {
                        Title = "Warning",
                        Description = "Are you sure you want to unlock this Hamster?",
                        YesButtonText = "Yes",
                        YesAction = async () => 
                        {
                            var result = await _playerManager.BuyCollectionItemInMarket(t.Value.collection_id);
                            Target.UpdateWindowView(_playerManager.MarketCollection);
                            
                            //CheckAchievementTarget
                            var achievementTargets = new List<AchievementTarget>();
                            achievementTargets.Add(new AchievementTarget(TargetType.BuyHamster, 1));
                            achievementTargets.Add(new AchievementTarget(TargetType.CollectHamsters, 1));
                            achievementTargets.Add(new AchievementTarget(TargetType.SpendBeans, result.collection_cost));
                            await _achievementManager.CheckAchievementTarget(achievementTargets.ToArray());
                        }
                    };
                    _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
                });
            }

            var poz = Target.CollectionItemTransform.position;
            Target.CollectionItemTransform.position = new Vector3(poz.x, 0, poz.z);
        }

        private void UpdateResource(int value, ResourcesType type)
        {
            Target.CurrencyItem.SetCurrencyData(value, type);
        }

        protected override void CloseStart()
        {
            base.CloseStart();
        }
    }
}