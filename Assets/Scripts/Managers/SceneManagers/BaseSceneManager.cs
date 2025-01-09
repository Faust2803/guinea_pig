using Game.Character;
using Game.Environment;
using Managers.SoundManager.Base;
using UnityEngine;
using Util;
using Zenject;

namespace Managers.SceneManagers
{
    public class BaseSceneManager : MonoBehaviour
    {
        [Inject] protected UiManager _uiManager;
        [Inject] protected PlayerManager _playerManager;
        [Inject] protected ISoundManager _audio;

        [SerializeField] protected Transform  _gameArea;

        
        
    }
}