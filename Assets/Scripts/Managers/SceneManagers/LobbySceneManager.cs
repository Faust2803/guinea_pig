using UI.Panels;


namespace Managers.SceneManagers
{
    public class LobbyBaseSceneManager : BaseSceneManager
    {
        private  void Start()
        {
            _uiManager.OpenPanel(PanelType.TopLobbyPanel);
            _uiManager.OpenPanel(PanelType.BottomLobbyPanel);

            _audio.PlaySound(SoundManager.Enums.SoundId.LobbyLoop, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.LobbyLoop, 0.5f);
           
        }
        private void Init()
        {
        }
        

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.LobbyLoop);
        }
    }
}
