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
        
        protected CharacterView CreateCharacter(CharacterModelType modelType, CharacterData data = null)
        {
            var playerCharacter = LoadCharacterPrefab(modelType);
            if (playerCharacter == null) return null;
            if (data != null)
            {
                playerCharacter.SetData(data);
            }
            return playerCharacter;
        }
        
        private CharacterView LoadCharacterPrefab(CharacterModelType modelType)
        {
            var view = _factoryCharacter.Create(modelType);
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