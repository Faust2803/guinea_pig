using System;
using Managers.SceneManagers;
using Zenject;

namespace UI.Panels.TopGamePanel
{
    public class TopGamePanelMediator : BasePanelMediator<TopGamePanelView, TopGamePanelData>
    {
        public event Action OnExit; 
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.Lifes.text = Data.lifes.ToString();
            Target.Boolets.text = Data.boolets.ToString();
            Target.Enemyes.text = Data.enemyes.ToString();
            Target.EndButton.onClick.AddListener(OnEndClicked);

            Data.player.OnLostLife += OnLostLife;
            Data.player.OnDeathEnemy += TurnOffDeathEnemy;
        }
        
        protected override void CloseStart()
        {
            base.CloseStart();
            Target.EndButton.onClick.RemoveListener(OnEndClicked);
            Data.player.OnLostLife -= OnLostLife;
            Data.player.OnDeathEnemy -= TurnOffDeathEnemy;
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
        
        private  void OnEndClicked ()
        {
            OnExit?.Invoke();
        }

        public void SetScore(int score)
        {
            Data.Score = score;
            Target.Score.text = Data.Score.ToString();
        }
    }
}