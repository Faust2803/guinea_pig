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