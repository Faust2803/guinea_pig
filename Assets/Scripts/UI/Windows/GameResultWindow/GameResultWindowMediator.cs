using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using Managers.SoundManager.Base;
using UI.Panels;
using Zenject;

namespace UI.Windows.GameResultWindow
{
    public class GameResultWindowMediator : BaseWindowMediator<GameResultWindowView, GameResultData>
    {
        private const string ResultTextFormat = "{0} km";

        [Inject] UiManager ui;
        [Inject] SceneLoadManagers scenes;
        [Inject] PlayerManager playerManager;
        [Inject] ISoundManager sounds;


        public override void SetData(object data)
        {
            base.SetData(data);
            sounds.StopSound(Managers.SoundManager.Enums.SoundId.JumperMusic);
        }

        

        private async void OnLobbyClicked ()
        {
            _uiManager.ClosePanel(PanelType.GamePanel);
            scenes.LoadScene(Scene.Lobby);
        }

        private async void OnRepeatClicked ()
        {
           
        }

        
    }
}