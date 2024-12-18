using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.BottomGamePanel
{
    public class BottomGamePanelMediator : BasePanelMediator<BottomGamePanelView, PanelData>
    {

        protected override void ShowStart()
        {
            base.ShowStart();

            Target.FireButton.onClick.AddListener(OnFireButton);
            Target.ReloadButton.onClick.AddListener(OnReloadButton);

        }

        protected override void CloseStart()
        {
            base.CloseStart();
            Target.FireButton.onClick.RemoveListener(OnFireButton);
            Target.ReloadButton.onClick.RemoveListener(OnReloadButton);
        }

        private void OnFireButton()
        {
            

        }
        
        private void OnReloadButton()
        {
            

        }
        

    }
}