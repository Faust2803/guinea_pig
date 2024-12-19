using Game.Character;
using Game.Environment;
using Managers.SoundManager.Base;
using UnityEngine;
using Util;
using Zenject;

namespace Managers.SceneManagers
{
    public abstract class BaseSceneManager : MonoBehaviour
    {
        [Inject] protected UiManager _uiManager;
        [Inject] protected PlayerManager _playerManager;
        [Inject] protected ISoundManager _audio;
        [Inject] protected FactoryCharacter _factoryCharacter;
        [Inject] protected FactoryEnvironment _factoryEnvironment;

        [SerializeField] protected Transform  _gameArea;

        protected abstract void Init();
        
        protected CharacterMediator CreateCharacter(CharacterType type, object data = null)
        {
            var playerCharacter = GetCharacter(type);
            if (playerCharacter == null) return null;
            playerCharacter.SetData(data);
            //playerCharacter.Show();
            return playerCharacter;
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
        
        protected EnvironmentView LoadEnvironmentPrefab(EnvironmentType type)
        {
            var view = _factoryEnvironment.Create(type);
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }
    }
}