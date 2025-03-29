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

            Data.player.OnLostLife += OnLostLife;
            Data.player.OnDeathEnemy += TurnOffDeathEnemy;
        }

        private void OnLostLife()
        {
            Data.lifes--;
            Target.Lifes.text = Data.lifes.ToString();
        }
        
        private void TurnOffDeathEnemy()
        {
            Data.enemyes--;
            Target.Enemyes.text = Data.enemyes.ToString();
        }
        
        
    }
}