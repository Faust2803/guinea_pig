using System.Collections.Generic;
using Game;
using Game.Character;
using Game.Character.Player;
using Game.Environment;
using UI.Panels;
using UnityEngine;
using Util;
using Zenject;


namespace Managers.SceneManagers
{
    public class GameBaseSceneManager : BaseSceneManager
    {
        [Inject] protected FactoryBoolet _factoryBoolet;
        
        private CharacterMediator _playerCharacterMediator;
        private Stack<GameObject> _booletPool = new Stack<GameObject>();
        
        
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
            _playerCharacterMediator = CreateCharacter(CharacterType.InGameCharacter);
            // CreateCharacter(CharacterType.Enemy1);
            // CreateCharacter(CharacterType.Enemy2);
            // CreateCharacter(CharacterType.Enemy3);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
            _playerCharacterMediator.Remove();
            _booletPool.Clear();
        }
        
        public void CreateBoolet(Vector3 position, Quaternion rotation)
        {
            GameObject boolet;
            if (_booletPool.Count == 0)
            {
                boolet = LoadBooletPrefab().gameObject;
            }
            else
            {
                boolet = _booletPool.Pop();
            }

            boolet.transform.position = position;
            boolet.transform.rotation = rotation;
            boolet.SetActive(true);
        }

        public void RemoveBoolet(GameObject boolet)
        {
            _booletPool.Push(boolet);
        }
        
        private BooletView LoadBooletPrefab()
        {
            var view = _factoryBoolet.Create();
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }
    }
}
