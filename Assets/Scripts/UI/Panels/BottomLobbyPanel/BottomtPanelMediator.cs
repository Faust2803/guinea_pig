using Managers;
using Managers.SceneManagers;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace UI.Panels.BootPanel
{
    public class BottomPanelMediator : BasePanelMediator<BottomPanelView, PanelData>
    {
        [Inject] private SceneLoadManagers _sceneLoadManagers;
        [Inject] private PlayerManager _playerManager;

        protected override void ShowStart()
        {
            base.ShowStart();

            Target.PlayWindowButton.onClick.AddListener(OnPlayButton);
            
            Target.SelectPersonagButton.onClick.AddListener(OnSelectPersonagButton);
            Target.SettingsButton.onClick.AddListener(OnSettingsButton);
            Target.UpgradeButton.onClick.AddListener(OnUpgradeButton);
            Target.ShopButton.onClick.AddListener(OnShopButton);
            
            Target.AchievmentsCompleatPointer(true);
        }

        protected override void CloseStart()
        {
            base.CloseStart();
            Target.PlayWindowButton.onClick.RemoveListener(OnPlayButton);
            
            Target.SelectPersonagButton.onClick.RemoveListener(OnSelectPersonagButton);
            Target.SettingsButton.onClick.RemoveListener(OnSettingsButton);
            Target.UpgradeButton.onClick.RemoveListener(OnUpgradeButton);
            Target.ShopButton.onClick.RemoveListener(OnShopButton);
        }

        private void OnPlayButton()
        {
            _uiManager.CloseAllPanels();
            _sceneLoadManagers.LoadScene(Scene.Game);
        }
        
        private void OnSelectPersonagButton()
        {
            _uiManager.OpenWindow(WindowType.SelectPersonagWindow);
        }
        
        private void OnSettingsButton()
        {
            _uiManager.OpenWindow(WindowType.SettingsWindow);
        }
        
        private void OnUpgradeButton()
        {
            _uiManager.OpenWindow(WindowType.UpgradeWindow);
        }
        
        private void OnShopButton()
        {
            _uiManager.OpenWindow(WindowType.ShopWindow);
        }
    }
}