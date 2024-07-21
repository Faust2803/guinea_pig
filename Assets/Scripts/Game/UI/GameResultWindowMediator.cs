using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataModels.Achievement;
using Managers;
using Managers.HamsterPreviewManager;
using Managers.SoundManager.Base;
using UI.Panels;
using UI.Windows;
using Zenject;

namespace Game.Jumper
{
    public class GameResultWindowMediator : BaseWindowMediator<GameResultWindowView, GameResultData>
    {
        private const string ResultTextFormat = "{0} km";

        [Inject] UiManager ui;
        [Inject] SceneLoadManagers scenes;
        [Inject] PlayerManager playerManager;
        [Inject] ISoundManager sounds;
        [Inject] private HamsterPreviewManager _hamsterPreviewManager;
        [Inject] private AchievementManager _achievementManager;

        public override void SetData(object data)
        {
            base.SetData(data);

            SetResult(Data.Player, Data.IsWin, Data.Score);
            _hamsterPreviewManager.ShowPreview(true);
            sounds.StopSound(Managers.SoundManager.Enums.SoundId.JumperMusic);
            //sounds.PlaySound(Managers.SoundManager.Enums.SoundId.FinalLoop);
        }

        private void SetResult(IPlayerBase player, bool isWinner, int score)
        {
            Target.SetupResult(player, isWinner);
            Target.LinkListeners(OnLobbyClicked, OnRepeatClicked);
            
            
            //CheckAchievementTarget
            var achievementTargets = new List<AchievementTarget>();

            achievementTargets.Add(isWinner
                ? new AchievementTarget(TargetType.AchieveMoon, 1)
                : new AchievementTarget(TargetType.DieXTimes, 1));
            achievementTargets.Add(new AchievementTarget(TargetType.EarnBeans, player.GoldenBeansCurrency));
            _achievementManager.CheckAchievementTarget(achievementTargets.ToArray());

            Target.Subtitle.text = string.Format(ResultTextFormat, NumberFormatter.FormatValue(score), player.GoldenBeansCurrency);
        }

        private async void OnLobbyClicked ()
        {
            _uiManager.ClosePanel(PanelType.GamePanel);
            scenes.LoadScene(Scene.Lobby);

            await DelayedClose();
        }

        private async void OnRepeatClicked ()
        {
            if(playerManager != null)
            {
                var weak = playerManager.CurrentCollectionItem.collection_current_hp <= 1;
                if(weak)
                {
                    ui.ForceOpenWindow(WindowType.AttentionWindow);
                }
                else
                {
                    scenes.ReloadScene();
                    await DelayedClose();
                }
            }
        }

        private async Task DelayedClose()
        {
            _hamsterPreviewManager.ShowPreview(false);
            await UniTask.Delay(50);
            CloseSelf();
        }
    }

    public class GameResultData : WindowData
    {
        public IPlayerBase Player;
        public bool IsWin;
        public int Score;

        public GameResultData (IPlayerBase player, bool win, int score)
        {
            Player = player;
            IsWin = win;
            Score = score;
        }
    }
    
    
    
}