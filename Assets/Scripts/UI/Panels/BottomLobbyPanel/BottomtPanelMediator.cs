
using Cysharp.Threading.Tasks;
using Managers;
using Managers.Analytics;
using System;
using UI.Windows;
using UI.Windows.HamsterOfMoonWindow;
using UI.Windows.SettingsWindow;
using UnityEngine;
using Zenject;

namespace UI.Panels.BootPanel
{
    public class BottomPanelMediator :BasePanelMediator <BottomPanelView, PanelData>
    {
        [Inject] private SceneLoadManagers _sceneLoadManagers;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private AchievementManager _achievementManager;
        [Inject] private INetworkManager _networkManager;

        protected override void ShowStart()
        {
            base.ShowStart();

            CheckAchievmentsCompleat();
            
            Target.CollectionWindowButton.onClick.AddListener(OnCollectionWindowButton);
            Target.MarketWindowButton.onClick.AddListener(OnMarketWindowButton);
            Target.NewsWindowButton.onClick.AddListener(OnNewsWindowButton);
            Target.PlayWindowButton.onClick.AddListener(OnPlayWindowButton);
            Target.ProfileWindowButton.onClick.AddListener(OnProfileWindowButton);
            Target.WalletWindowButton.onClick.AddListener(OnWalletWindowButton);
            Target.SettingsWindowButton.onClick.AddListener(OnSettingWindowButton);
            Target.ForTestingButton.onClick.AddListener(ForTestingButton);
            Target.ResetProgressButton.onClick.AddListener(ResetProgressButton);
            Target.TamagochWindowButton.onClick.AddListener(TamagochButton);
            Target.LeaderboardWindowButton.onClick.AddListener(LeaderboardButton);
            Target.AchievmentsWindowButton.onClick.AddListener(AchievemntsButton);
            Target.PiramidWindowButton.onClick.AddListener(PiramidButton);
            Target.HamsterOfMoonButton.onClick.AddListener(HamsterOfMoon);

            _achievementManager.TaskComplete += CheckAchievmentsCompleat;
            _achievementManager.AchievementComplete += CheckAchievmentsCompleat;
        }

        private async void CheckAchievmentsCompleat()
        {
            var result = await _achievementManager.ReadyToCompleted();
            Target.AchievmentsCompleatPointer(result.Count > 0);
        }
        
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            Target.CollectionWindowButton.onClick.RemoveListener(OnCollectionWindowButton);
            Target.MarketWindowButton.onClick.RemoveListener(OnMarketWindowButton);
            Target.NewsWindowButton.onClick.RemoveListener(OnNewsWindowButton);
            Target.PlayWindowButton.onClick.RemoveListener(OnPlayWindowButton);
            Target.ProfileWindowButton.onClick.RemoveListener(OnProfileWindowButton);
            Target.WalletWindowButton.onClick.RemoveListener(OnWalletWindowButton);
            Target.SettingsWindowButton.onClick.RemoveListener(OnSettingWindowButton);
            Target.ForTestingButton.onClick.RemoveListener(ForTestingButton);
            Target.ResetProgressButton.onClick.RemoveListener(ResetProgressButton);
            Target.TamagochWindowButton.onClick.RemoveListener(TamagochButton);
            Target.LeaderboardWindowButton.onClick.RemoveListener(LeaderboardButton);
            Target.AchievmentsWindowButton.onClick.RemoveListener(AchievemntsButton);
            Target.PiramidWindowButton.onClick.RemoveListener(PiramidButton);
            Target.HamsterOfMoonButton.onClick.RemoveListener(HamsterOfMoon);
            
            _achievementManager.TaskComplete -= CheckAchievmentsCompleat;
            _achievementManager.AchievementComplete -= CheckAchievmentsCompleat;
        }

        private async void ResetProgressButton()
        {
            var response = await _playerManager.ResetUser();

            if (response)
            {
                PlayerPrefs.DeleteAll();

                _uiManager.CloseAllPanels();

                _sceneLoadManagers.LoadScene(Scene.Boot);

            }
        }

        private void OnCollectionWindowButton()
        {
            _uiManager.OpenWindow(WindowType.CollectionWindow);
            _analyticsManager.ViewCollection();
        }
        
        private void OnWalletWindowButton()
        {
            _uiManager.OpenWindow(WindowType.WalletWindow);
        }

        private void OnMarketWindowButton()
        {
            _uiManager.OpenWindow(WindowType.MarketWindow);
        }

        private void OnNewsWindowButton()
        {
            _uiManager.OpenWindow(WindowType.NewsWindow);
        }

        private async void OnPlayWindowButton()
        {
            
            _sceneLoadManagers.LoadScene(Scene.Game);
            await UniTask.Delay(50);
            CloseSelf();

            _uiManager.ClosePanel(PanelType.TopLobbyPanel);
            _uiManager.ClosePanel(PanelType.TamagochiPanel);
        }

        private void OnProfileWindowButton()
        {
            _uiManager.OpenWindow(WindowType.ProfileWindow);
            _analyticsManager.ViewProfile();
        }

        private void OnSettingWindowButton()
        {
            _uiManager.OpenWindow(WindowType.Settings);
        }
        
        private void ForTestingButton()
        {
            _uiManager.OpenWindow(WindowType.WalletWindow);
        }
        
        private void TamagochButton()
        {
            _uiManager.OpenWindow(WindowType.TamagochWindow);
        }
        
        private void LeaderboardButton()
        {
            _uiManager.OpenWindow(WindowType.LeaderboardWindow);
            _analyticsManager.ViewLeaderboard();
        }

        private void AchievemntsButton()
        {
            _uiManager.OpenWindow(WindowType.AchievementsWindow);
            _analyticsManager.ViewQuests();
        }

        private void PiramidButton()
        {
            _uiManager.OpenWindow(WindowType.PyramidWindow);
            _analyticsManager.AirdropInteraction("view_screen");
        }

        private void HamsterOfMoon()
        {
            HamsterOfTheMoon();
        }

        private async UniTaskVoid HamsterOfTheMoon()
        {
            Target.HamsterOfMoonButton.interactable = false;
            var result = await _networkManager.GetMoonInfo();
            
            Target.HamsterOfMoonButton.interactable = true;

            if(result == null) return;

            Sprite iconSprite = null;
            if (int.TryParse(result.Equipment.CollectionId, out var id))
                if (_playerManager.PlayerCollection.TryGetValue(id, out var collectionData))
                    iconSprite = collectionData.spriteIcon;
            
            var windowData = new HamsterOfMoonData(result.UserName, result.BankAmount, iconSprite);
            _uiManager.OpenWindow(WindowType.HamsterOfMoonWindow, windowData);
        }
    }
}