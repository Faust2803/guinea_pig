using System.Collections.Generic;
using Game.Jumper;
using Managers;
using Managers.SoundManager.Base;
using UI.Windows.SettingsWindow;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Windows.PauseWindow
{
    public class PauseWindowMediator : BaseWindowMediator<PauseWindowView, PauseWindowData>
    {
        [Inject] private ISoundManager _soundManager;
        [Inject] private SceneLoadManagers _sceneManager;

        private List<SoundSettingWrapper> _wrappers = new List<SoundSettingWrapper>();
        
        protected override void ShowStart()
        {
            base.ShowStart();
            
            var soundData = _soundManager.GetVolumeSettings();

            foreach (var data in soundData)
            {
                var viewData = Target.Config.GetViewData(data.SoundType);

                var slider = Object.Instantiate(Target.BtnSliderPrefab, Target.ParentForSettings);
                var wrapper = new SoundSettingWrapper(data, _soundManager, viewData, slider);
                _wrappers.Add(wrapper);
            }
            
            Target.BtnContinue.onClick.AddListener(OnContinueClick);
            Target.BtnToLobby.onClick.AddListener(OnLobbyClick);
        }

        protected override void ShowEnd()
        {
            SetPause(true);
            base.ShowEnd();
        }

        private void OnContinueClick()
        {
            CloseSelf();
        }

        private void OnLobbyClick()
        {
            CloseSelf(() =>
            {
                JumperSoloGameController.CanselGame();
                // _sceneManager.LoadScene(Scene.Lobby);
            });
        }

        private void SetPause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
            AudioListener.pause = isPaused;
        }

        protected override void CloseStart()
        {
            base.CloseStart();
            SetPause(false);
        }
        
        protected override void CloseFinish()
        {
            base.CloseFinish();
            OnClose();
        }

        private void OnClose()
        {
            foreach (var wrapper in _wrappers)
            {
                wrapper.Dispose();
                Object.Destroy(wrapper.BtnSlider.gameObject);
            }
            
            _wrappers.Clear();
        }
    }
    
    public class PauseWindowData : WindowData
    {
        
    }
}