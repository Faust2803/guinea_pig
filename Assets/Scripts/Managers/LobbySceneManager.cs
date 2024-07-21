using Managers.Analytics;
using Managers.SoundManager.Base;
using UI.Panels;
using UI.Windows;
using UI.Windows.RewardWindow;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LobbySceneManager : MonoBehaviour
    {
        [Inject] private UiManager _uiManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private AchievementManager _achievementManager;
        [Inject] private TutorialManager _tutorialManager;
        [Inject] private ISoundManager _audio;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private LeaderboardManager _leaderboardManager;
        
        private async void Start()
        {
            await _playerManager.UpdatePlayerData();
            await _leaderboardManager.GetLeaderboard();
            
            
            _uiManager.OpenPanel(PanelType.TopLobbyPanel);
            _uiManager.OpenPanel(PanelType.BottomLobbyPanel);
            _uiManager.OpenPanel(PanelType.TamagochiPanel);
            _analyticsManager.Lobby();
            
            _audio.PlaySound(SoundManager.Enums.SoundId.LobbyLoop, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.LobbyLoop, 0.5f);

            // var windowData = new RewardWindowData
            // {
            //     Title = "Reward",
            //     Description = $"You have successfully finished your goal! Please accept this reward!",
            //     NoButtonActive = false,
            //     YesButtonText = "CLAIM",
            //     //HamstaImage = _playerManager.CurrentCollectionItem.spriteIcon,
            //     //ResourceImage = _playerResourcesIconConfig.GetIconSprite(ResourcesType.GoldenBean),
            //     ResourceValue = 10,
            // };
            // _uiManager.ForceOpenWindow(WindowType.RewardWindow, windowData);
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.LobbyLoop);
        }
    }
}
