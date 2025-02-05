using System;
using Cysharp.Threading.Tasks;
using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.BottomGamePanel
{
    public class BottomGamePanelMediator : BasePanelMediator<BottomGamePanelView, PanelData>
    {
        public static event Action OnFire;
        public static event Action OnReload;

        private int _sootingDelay = 1000;
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.FireButton.onClick.AddListener(OnFireButton);
            Target.ReloadButton.onClick.AddListener(OnReloadButton);
            Target.FireButtonLockImage.SetActive(false);

        }

        protected override void CloseStart()
        {
            base.CloseStart();
            Target.FireButton.onClick.RemoveListener(OnFireButton);
            Target.ReloadButton.onClick.RemoveListener(OnReloadButton);
        }

        private void OnFireButton()
        {
            OnFire?.Invoke();
            Target.FireButton.enabled = false;
            Target.FireButtonLockImage.SetActive(true);
            
            ShootingDelay().Forget();
        }
        
        private void OnReloadButton()
        {
            OnReload?.Invoke();
            
        }

        private async UniTask ShootingDelay()
        {
            await UniTask.Delay(_sootingDelay);
            Target.FireButton.enabled = true;
            Target.FireButtonLockImage.SetActive(false);
        } 
    }
}