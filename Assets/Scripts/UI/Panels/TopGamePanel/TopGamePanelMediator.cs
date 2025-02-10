using System;
using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.TopGamePanel
{
    public class TopGamePanelMediator : BasePanelMediator<TopGamePanelView, PanelData>
    {
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.Lifes.text = "5";
            Target.Boolets.text = "4";
            Target.Enemyes.text = "7";
        }
    }
}