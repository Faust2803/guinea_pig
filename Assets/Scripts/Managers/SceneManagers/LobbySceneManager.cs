using Managers.SoundManager.Base;
using UI.Panels;
using UnityEngine;
using Zenject;

namespace Managers.SceneManagers
{
    public class LobbySceneManager : MonoBehaviour
    {
        [Inject] private UiManager _uiManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private ISoundManager _audio;

        
        private async void Start()
        {
            await _playerManager.UpdatePlayerData();
            
            _uiManager.OpenPanel(PanelType.TopLobbyPanel);
            _uiManager.OpenPanel(PanelType.BottomLobbyPanel);

            _audio.PlaySound(SoundManager.Enums.SoundId.LobbyLoop, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.LobbyLoop, 0.5f);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.LobbyLoop);
        }
    }
}
