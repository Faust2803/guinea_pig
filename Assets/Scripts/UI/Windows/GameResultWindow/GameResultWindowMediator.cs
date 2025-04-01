using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using Managers.SceneManagers;
using Managers.SoundManager.Base;
using UI.Panels;
using Zenject;

namespace UI.Windows.GameResultWindow
{
    public class GameResultWindowMediator : BaseWindowMediator<GameResultWindowView, GameResultData>
    {
        [Inject] SceneLoadManagers scenes;
        [Inject] PlayerManager playerManager;
        [Inject] ISoundManager sounds;
        
        public override void SetData(object data)
        {
            base.SetData(data);
            sounds.StopSound(Managers.SoundManager.Enums.SoundId.JumperMusic);
        }
        
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.LobbyButton.onClick.AddListener(OnLobbyClicked);
            Target.RepeatButton.onClick.AddListener(OnRepeatClicked);

            Target.IsWin.text = Data.IsWin ? "You won" : "You lose";
            Target.Lives.text = $"Life Count {Data.Lives.ToString()}";
            Target.Score.text = $"Score received {Data.Score.ToString()}";
            Target.Enemyes.text = $"Enemyes destroyed {Data.Enemyes.ToString()}";
            Target.Bullets.text = $"Bullet used {Data.Bullets.ToString()}";
            
        }
        
        protected override void CloseStart()
        {
            base.CloseStart();
            Target.LobbyButton.onClick.RemoveListener(OnLobbyClicked);
            Target.RepeatButton.onClick.RemoveListener(OnRepeatClicked);
        }

        private  void OnLobbyClicked ()
        {
            scenes.LoadScene(Scene.Lobby);
            _uiManager.CloseAllPanels();
            CloseSelf();
        }
        
        private void OnRepeatClicked ()
        {
            scenes.ReloadScene();
            _uiManager.CloseAllPanels();
            CloseSelf();
            
        }
        
       


        
    }
}