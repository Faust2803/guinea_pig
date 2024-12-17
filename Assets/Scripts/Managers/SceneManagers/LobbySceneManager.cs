using Game.Character;
using Game.Environment;
using Managers.SoundManager.Base;
using UI.Panels;


namespace Managers.SceneManagers
{
    public class LobbyBaseSceneManager : BaseSceneManager
    {
        private async void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            _uiManager.OpenPanel(PanelType.TopLobbyPanel);
            _uiManager.OpenPanel(PanelType.BottomLobbyPanel);

            _audio.PlaySound(SoundManager.Enums.SoundId.LobbyLoop, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.LobbyLoop, 0.5f);
            Init();
        }

        protected override void Init()
        {
            LoadEnvironmentPrefab(EnvironmentType.LobbyEnvironment);
            CreateCharacter(CharacterType.LobbyPlayerCharacter);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.LobbyLoop);
        }
    }
}
