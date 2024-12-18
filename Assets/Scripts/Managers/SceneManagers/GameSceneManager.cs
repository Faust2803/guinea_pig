using Game.Character;
using Game.Environment;
using UI.Panels;


namespace Managers.SceneManagers
{
    public class GameBaseSceneManager : BaseSceneManager
    {
        private async void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            _uiManager.OpenPanel(PanelType.BottomGamePanelView);

            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
        }

        protected override void Init()
        {
            LoadEnvironmentPrefab(EnvironmentType.Environment1);
            CreateCharacter(CharacterType.InGameCharacter);
            // CreateCharacter(CharacterType.Enemy1);
            // CreateCharacter(CharacterType.Enemy2);
            // CreateCharacter(CharacterType.Enemy3);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
        }
    }
}
