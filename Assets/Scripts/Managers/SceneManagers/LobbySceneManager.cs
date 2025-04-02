using System;
using Game.Character;
using Game.Environment;
using UI.Panels;
using UnityEngine;


namespace Managers.SceneManagers
{
    public class LobbyBaseSceneManager : BaseSceneManager
    {
        private void Awake()
        {
#if UNITY_ANDROID
            Input.gyro.enabled = false; // Отключает гироскоп
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
#endif
        }

        private void Start()
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
            CreateCharacter(CharacterModelType.LobbyPlayerCharacter);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.LobbyLoop);
        }
    }
}
