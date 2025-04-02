using System.Collections.Generic;
using Game;
using Game.Character;
using Game.Character.Player;
using Game.Environment;
using UI.Panels;
using UI.Panels.BottomGamePanel;
using UI.Panels.TopGamePanel;
using UI.Windows;
using UI.Windows.GameResultWindow;
using UnityEngine;
using Util;
using Zenject;

namespace Managers.SceneManagers
{
    public class GameSceneManager : BaseSceneManager
    {
        [Inject] protected FactoryBoolet _factoryBoolet;
        
        private PlayerCharacterView _playerCharacter;
        private List<CharacterView>  _enemyCharacterList = new List<CharacterView>();
        private Stack<BooletView> _booletPool = new Stack<BooletView>();
        public EnvironmentView EnvironmentView { get; private set; }
        
        private int _playerCounter = 0;
        private int _enemyCounter = 0;
        
        private Dictionary<CharacterView, int> _score = new Dictionary<CharacterView, int>();
        private TopGamePanelMediator _topGamePanel;
        private BottomGamePanelMediator _bottomGamePanel;
        
        // Need add to config
        private int _killScoreFactor = 10;
        
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
#if UNITY_ANDROID
            Input.gyro.enabled = false;
            Application.targetFrameRate = 90;
#endif
#if UNITY_EDITOR
            Application.targetFrameRate = -1;
#endif
        }
        
        private void Start()
        {
            //await _playerManager.UpdatePlayerData();
            
            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
            _bottomGamePanel = _uiManager.OpenPanel(PanelType.BottomGamePanel)as BottomGamePanelMediator;
            _topGamePanel = _uiManager.OpenPanel(PanelType.TopGamePanel, 
                new TopGamePanelData{lifes = _playerCharacter.Lives, boolets = 100, enemyes = EnvironmentView.SpawnPoint.Count, player = _playerCharacter}
                ) as TopGamePanelMediator;
            _topGamePanel.OnExit += OnExit;
            _playerCharacter.SetUiButtons(_bottomGamePanel);
            _score.Clear();
        }

        protected override void Init()
        {
            EnvironmentView = LoadEnvironmentPrefab(EnvironmentType.Environment1);
            _playerCharacter = CreateCharacter(CharacterModelType.InGameCharacter,
                new CharacterData { transform = EnvironmentView.PlayerSpawnPoint, lifes = 5, boollets = 100, type = CharacterType.Player}
                ) as PlayerCharacterView;
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
            _topGamePanel.OnExit -= OnExit;
        }
        
        public void CreateBoolet(Vector3 position, Quaternion rotation, GameCharacterView shooter)
        {
            BooletView boolet;
            if (_booletPool.Count == 0)
            {
                boolet = LoadBooletPrefab();
            }
            else
            {
                boolet = _booletPool.Pop();
            }
            boolet.SetData(shooter, position, rotation);
        }

        public void RemoveBoolet(BooletView boolet)
        {
            _booletPool.Push(boolet);
        }
        
        private BooletView LoadBooletPrefab()
        {
            var view = _factoryBoolet.Create();
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }

        public void TurnOffDeathCharacter(GameCharacterView character, CharacterView shooter)
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

            if (!_score.TryAdd(shooter, 1))
            {
                _score[shooter] ++;
            }

            if (shooter  == _playerCharacter)
            {
                _topGamePanel.SetScore(_score[shooter]*_killScoreFactor);
            }
        }

        public void CheckEndLevel()
        {
            if (_playerCounter == 0)
            {
                OpenEndGameWindow(false);
                return;
            }
            if (_enemyCounter == 0)
            {
                OpenEndGameWindow(true);
                return;
            }
        }

        private void OpenEndGameWindow(bool isWin)
        {
            var score = 0;
            var enemyes = 0;
            if (_score.ContainsKey(_playerCharacter))
            {
                score = _score[_playerCharacter] * _killScoreFactor;
                enemyes = _score[_playerCharacter];
            }
            
            _uiManager.OpenWindow(WindowType.GameResultWindow,
                new GameResultData
                {
                    IsWin = isWin, 
                    Score = score,
                    Enemyes = enemyes,
                    Bullets = 100,
                    Lives = _playerCharacter.Lives
                });
        }

        private void OnExit()
        {
            OpenEndGameWindow(false);
        }
    }
}
