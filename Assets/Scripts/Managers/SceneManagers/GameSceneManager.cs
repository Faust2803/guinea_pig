using System.Collections.Generic;
using Game;
using Game.Character;
using Game.Environment;
using UI.Panels;
using UnityEngine;
using Util;
using Zenject;


namespace Managers.SceneManagers
{
    public class GameSceneManager : BaseSceneManager
    {
        [Inject] protected FactoryBoolet _factoryBoolet;
        
        private CharacterView _playerCharacter;
        private List<CharacterView>  _enemyCharacterList = new List<CharacterView>();
        private Stack<GameObject> _booletPool = new Stack<GameObject>();
        public EnvironmentView EnvironmentView { get; private set; }
        
        private int _playerCounter = 0;
        private int _enemyCounter = 0;
        
        private void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
            _uiManager.OpenPanel(PanelType.BottomGamePanelView);
            _uiManager.OpenPanel(PanelType.TopGamePanel, new TopGamePanelData{lifes = 5, boolets = 100, enemyes = EnvironmentView.SpawnPoint.Count, player = _playerCharacter});
        }

        protected override void Init()
        {
            EnvironmentView = LoadEnvironmentPrefab(EnvironmentType.Environment1);
            _playerCharacter = CreateCharacter(CharacterModelType.InGameCharacter,
                new CharacterData { transform = EnvironmentView.PlayerSpawnPoint, lifes = 5, boollets = 100, type = CharacterType.Player}
                );
            _playerCounter = 1;
            for (var i = 0; i < EnvironmentView.SpawnPoint.Count; i++)
            {
                var enemy = CreateCharacter(EnvironmentView.EnemyType[i],
                    new CharacterData { transform = EnvironmentView.SpawnPoint[i], lifes = 1, type = CharacterType.Enemy}
                    );
                _enemyCharacterList.Add(enemy);
            }
            _enemyCounter = _enemyCharacterList.Count;
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
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
            boolet.SetActive(false);
        }
        
        private BooletView LoadBooletPrefab()
        {
            var view = _factoryBoolet.Create();
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }

        public void TurnOffDeathCharacter(GameCharacterView character)
        {
            if (character.CharacterType == CharacterType.Player)
            {
                _playerCounter--;
                for (var i = 0; i < _enemyCharacterList.Count; i++)
                {
                    _enemyCharacterList[i].TurnOffDeathPlayer(character, _playerCounter);
                }
            }
            else
            {
                _enemyCounter--;
                _playerCharacter.TurnOffDeathPlayer(character, _enemyCounter);
            }
        }

        public void CheckEndLevel()
        {
            if (_playerCounter == 0)
            {
                Debug.Log("defeat");
                return;
            }
            if (_enemyCounter == 0)
            {
                Debug.Log("victory");
                return;
            }
        }
    }
}
