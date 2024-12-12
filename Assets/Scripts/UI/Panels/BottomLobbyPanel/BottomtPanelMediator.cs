using Managers;
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
        }

        protected override void CloseStart()
        {
            base.CloseStart();
            Target.PlayWindowButton.onClick.RemoveListener(OnPlayButton);
        }

        private void OnPlayButton()
        {
            _uiManager.CloseAllPanels();
            _sceneLoadManagers.LoadScene(Scene.Game);
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

    }
}