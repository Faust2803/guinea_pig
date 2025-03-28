using System;
using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.TopGamePanel
{
    public class TopGamePanelMediator : BasePanelMediator<TopGamePanelView, TopGamePanelData>
    {
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.Lifes.text = Data.lifes.ToString();
            Target.Boolets.text = Data.boolets.ToString();
            Target.Enemyes.text = Data.enemyes.ToString();
        }
    }
}