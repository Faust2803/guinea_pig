using Game.Character;
using Game.Character.Player;
using Game.Environment;
using Managers.SoundManager.Base;
using UI.Windows;
using UnityEngine;
using Util;
using Zenject;

namespace Managers.SceneManagers
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
            
            LoadEnvironmentPrefab(EnvironmentType.Environment1);
            CreatePlayerCharacter(CharacterType.InGameCharacter);
            CreatePlayerCharacter(CharacterType.Enemy1);
            CreatePlayerCharacter(CharacterType.Enemy2);
            CreatePlayerCharacter(CharacterType.Enemy3);


        }
        
        private void CreatePlayerCharacter(CharacterType type, object data = null)
        {
            var playerCharacter = GetCharacter(type);
            if (playerCharacter == null) return;
            playerCharacter.SetData(data);
            //playerCharacter.Show();
        }
        
        private CharacterMediator GetCharacter(CharacterType type)
        {
            var view = LoadCharacterPrefab(type);
            if (view == null) return null;
        
            view.Init();
            CharacterMediator mediator;
            view.OnCreateMediator(out mediator);
            mediator.SetType(type);
            
            return mediator;
        }
        
        private CharacterView LoadCharacterPrefab(CharacterType type)
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
