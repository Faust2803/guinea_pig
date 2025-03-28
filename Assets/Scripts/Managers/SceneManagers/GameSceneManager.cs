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
        
        private void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
            _uiManager.OpenPanel(PanelType.BottomGamePanelView, new TopGamePanelData{lifes = 5, boolets = 100, enemyes = EnvironmentView.SpawnPoint.Count});
            _uiManager.OpenPanel(PanelType.TopGamePanel);
        }

        protected override void Init()
        {
            EnvironmentView = LoadEnvironmentPrefab(EnvironmentType.Environment1);
            _playerCharacter = CreateCharacter(CharacterType.InGameCharacter,
                new CharacterData { transform = EnvironmentView.PlayerSpawnPoint, lifes = 5, boollets = 100});
            for (var i = 0; i < EnvironmentView.SpawnPoint.Count; i++)
            {
                var enemy = CreateCharacter(EnvironmentView.EnemyType[i],
                    new CharacterData { transform = EnvironmentView.SpawnPoint[i], lifes = 1});
                _enemyCharacterList.Add(enemy);
            }
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
    }
}
