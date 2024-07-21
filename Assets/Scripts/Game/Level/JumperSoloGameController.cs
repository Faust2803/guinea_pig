using Common.Signals.StatisticSignal;
using Common.Statistic;
using Cysharp.Threading.Tasks;
using DataModels.GameData;
using Managers;
using Managers.Analytics;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using Zenject;

namespace Game.Jumper
{
    public class JumperSoloGameController : MonoBehaviour
    {
        [SerializeField] JumperSoloPlayer player;

        [Inject] UiManager ui;
        [Inject] ISoundManager _sounds;
        [Inject] INetworkManager networkManager;
        [Inject] private SignalBus _signalBus;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private PlayerManager _playerManager;


        public GameLayerController Layers;
        public Vector2 MinAndMaxPlayerX = new Vector2(-7, 7);

        public ValueByType<bool> IsGameBegin = new ValueByType<bool>();
        public ValueByType<bool> StartedByPlayer = new ValueByType<bool>();
        public ValueByType<bool> IsGameEnded = new ValueByType<bool>();
        private int lostHp = 0;

        public StartGameDataModel StartData;

        private void Start()
        {
            //Application.targetFrameRate = 75;
            BeginGame();
        }

        public static void CanselGame()
        {
            var game = GameObject.FindWithTag("GameController");
            var controller = game.GetComponent<JumperSoloGameController>();
            controller.CancelGame();
        }

        private async void BeginGame()
        {
            lostHp = 0;
            StartData = await networkManager.StartGame();
            if(StartData != null && StartData.success)
            {
                IsGameBegin.Value = true;
                Debug.Log($"Game success begined StartData.game_id = {StartData.game_id}");
                player.Health.OnChangedBy += PlayerHealthChanged;
            }
            else
            {
                Debug.Log($"Game not started. Answer from network manager: {(StartData != null ? JsonUtility.ToJson(StartData) : "null")}");
            }
            _sounds.PlaySound(SoundId.JumperMusic, true);
            _signalBus.Fire(new StatisticSignal(StatisticSourceId.PlayGame, 1));
        }

        private async void PlayerHealthChanged(string attacker, int old, int now)
        {
            lostHp = lostHp + (old - now);
            if (now <= 0)
            {
                PlayerDeath();
            }

            Debug.Log($"Send health change analytics: {attacker}({now})");
            _analyticsManager.LostHp(attacker, now);
            
            var result = await _playerManager.LostHp(-1, (old - now));
            StartData.moon_bank = result.moon_bank;
            Layers.spawner.UpdateMoonBank();
        }

        private void PlayerDeath()
        {
            GameEndByResult(false);
        }

        private async void GameEndByResult(bool looseOrWin)
        {
            IsGameEnded.Value = true;
            _analyticsManager.EndGamesJamperSolo(looseOrWin, player.GoldenBeansCurrency);
            
            await UniTask.Delay(1000);

            StopAllMusic();
            _sounds.PlayOneShot(looseOrWin ? SoundId.Victory : SoundId.Loose);

            //Application.targetFrameRate = 30;

            ui.OpenWindow(UI.Windows.WindowType.JumperGameWindow, new GameResultData(player, looseOrWin, Layers.ConvertToGameMetersFromPos(player.heightestPos)));
            _signalBus.Fire(new SaveStatisticSignal());
            
            var result = await networkManager.EndGame(new EndGameDataModel() {
                collected_beans = player.GoldenBeansCurrency,
                moon_reached = looseOrWin,
                score = (int)player.transform.position.y,
                lost_hp = lostHp,
                game_id = StartData.game_id
            });
        }

        private void StopAllMusic ()
        {
            for(var x = 0; x < Layers.Map.Length; x++)
            {
                foreach(var audio in Layers.Map[x].Audio)
                {
                    if(_sounds.IsPlaying(audio.Sound))
                        _sounds.StopSound(audio.Sound);
                }
            }
        }
        
        private async void CancelGame()
        {
            IsGameEnded.Value = true;
            ui.OpenWindow(UI.Windows.WindowType.JumperGameWindow, new GameResultData(player, false, Layers.ConvertToGameMetersFromPos(player.heightestPos)));
            _signalBus.Fire(new SaveStatisticSignal());
            
            var result = await networkManager.CanceGame(new EndGameDataModel() {
                collected_beans = player.GoldenBeansCurrency,
                moon_reached = false,
                score = (int)player.transform.position.y,
                lost_hp = lostHp,
                game_id = StartData.game_id
            });
        }

        public void PlayerWin()
        {
            GameEndByResult(true);
        }

        private void OnDestroy() => _sounds.StopAllSounds();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (player == null)
            {
                var tmp = GameObject.FindWithTag("Player");
                if (tmp) player = tmp.GetComponent<JumperSoloPlayer>();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var left = new Vector3(MinAndMaxPlayerX.x, 0);
            var right = new Vector3(MinAndMaxPlayerX.y, 0);
            Gizmos.DrawLine(left, left + Vector3.up * 500);
            Gizmos.DrawLine(right, right + Vector3.up * 500);
        }
#endif
    }
}