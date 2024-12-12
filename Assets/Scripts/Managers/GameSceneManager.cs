using Game;
using Managers.SoundManager.Base;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Util;
using Zenject;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        [Inject] private UiManager _uiManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private ISoundManager _audio;
        [Inject] private FactoryCharacter _factoryCharacter;
        [Inject] private FactoryEnvironment _factoryEnvironment;

        [SerializeField] private Transform  _gameArea;
        
        private async void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            //_uiManager.OpenPanel(PanelType.TopLobbyPanel);

            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            var environment = LoadEnvironmentPrefab(EnvironmentType.Environment1);
            var character = LoadCharacterPrefab(CharacterType.InGameCharacter);
        }
        
        private BaseCharacterView LoadCharacterPrefab(CharacterType type)
        {
            var view = _factoryCharacter.Create(type);
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }
        
        private EnvironmentView LoadEnvironmentPrefab(EnvironmentType type)
        {
            var view = _factoryEnvironment.Create(type);
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
        }
    }
}
