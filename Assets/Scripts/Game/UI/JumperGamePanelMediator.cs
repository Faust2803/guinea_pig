using Cysharp.Threading.Tasks;
using Managers;
using Managers.Analytics;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Game.Jumper
{
    public class JumperGamePanelMediator : BasePanelMediator<JumperSoloGamePanel, PanelData>
    {
        [Inject] private UiManager _uiManager;
        [Inject] private TutorialManager _tutorialManager;
        [Inject] private AnalyticsManager _analyticsManager;

        private int ShpereId = -1;
        
        protected override void ShowStart()
        {
            base.ShowStart();
            
            _analyticsManager.StartGamesJamperSolo();
            
            Target.BackButton.onClick.AddListener(CanselGame);
            Target.PeanButtonTutorial.SetActive(false);
            Target.PeanStartTutorial.SetActive(false);
            Target.ToTheRightTutorial.SetActive(false);
            Target.ToTheLeftTutorial.SetActive(false);
            
            Target.CornButton.onClick.AddListener(SendanalyticsCorn);
            Target.PeanButton.onClick.AddListener(SendanalyticsPean);
            Target.SeedButton.onClick.AddListener(SendanalyticsSeed);

            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.ToTheMoonFirstStart))
            {
                StartTutorial();
            }
            Target.peanAbility.buttonComponent.onClick.AddListener(LaunchHamsta);
        }
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            Target.BackButton.onClick.RemoveListener(CanselGame);
            
            Target.CornButton.onClick.RemoveListener(SendanalyticsCorn);
            Target.PeanButton.onClick.RemoveListener(SendanalyticsPean);
            Target.SeedButton.onClick.RemoveListener(SendanalyticsSeed);
            
            if(Target.JumperSoloPlayer)
                Target.JumperSoloPlayer.OnPlatformInteract -= OnLendedForAnalytics;
        }

        private void SendanalyticsCorn()
        {
            _analyticsManager.UseAbility("use_corns");
        }
        
        private void SendanalyticsPean()
        {
            _analyticsManager.UseAbility("use_peas");
        }
        
        private void SendanalyticsSeed()
        {
            _analyticsManager.UseAbility("use_seeds");
        }
        
        private  void CanselGame()
        {
            _uiManager.OpenWindow(WindowType.Pause);
        }
        
        
        private void LaunchHamsta()
        {
            Target.peanAbility.buttonComponent.onClick.RemoveListener(LaunchHamsta);
            _analyticsManager.PlayGamesJamperSolo();
            
            Target.JumperSoloPlayer.OnPlatformInteract += OnLendedForAnalytics;
        }
        
        private  void OnLendedForAnalytics(JumperSoloPlatform platform)
        {
            if (Target.ShpereId != ShpereId)
            {
                _analyticsManager.CheckPointJamperSolo(Target.ShpereId);
                ShpereId = Target.ShpereId;
                Debug.Log($"ShpereId == {ShpereId}");
            }
        }
        
        private async void StartTutorial()
        {
            Target.CornButtonContent.SetActive(false);
            Target.SeedButtonContent.SetActive(false);
            
            Target.peanAbility.buttonComponent.onClick.AddListener(OnClickPeanAbility);
            await UniTask.Delay(3000);
            
            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.ToTheMoonFirstStart))
            {
                Target.PeanButtonTutorial.SetActive(true);
            }
        }

        private void OnClickPeanAbility()
        {
            _tutorialManager.SaveTutorialSteps(TutorialSteps.ToTheMoonFirstStart);
            Target.CornButtonContent.SetActive(true);
            Target.SeedButtonContent.SetActive(true);
            Target.PeanButtonTutorial.SetActive(false);
            Target.peanAbility.buttonComponent.onClick.RemoveListener(OnClickPeanAbility);
            
            Target.JumperSoloPlayer.Peas.IsActivated.OnValueChanged += OnStateChanged;
        }
        
        private void OnStateChanged(bool oldActive, bool nowActive)
        {
            if (oldActive && !nowActive)
            {
                Target.JumperSoloPlayer.Peas.IsActivated.OnValueChanged -= OnStateChanged;
                Target.PeanStartTutorial.SetActive(true);
                Time.timeScale = 0;
                Target.joystick.Negative.OnClicked += ToTheLeft;
                Target.joystick.Positive.OnClicked += ToTheRight;
            }
        }

        private int _tutorialClickTurn = 0;

        private void ToTheRight()
        {
            Target.joystick.Negative.OnClicked -= ToTheLeft;
            Target.joystick.Positive.OnClicked -= ToTheRight;
            Time.timeScale = 1;
            Target.PeanStartTutorial.SetActive(false);
            _tutorialManager.SaveTutorialSteps(TutorialSteps.ToTheMoonFirstJump);
            Target.JumperSoloPlayer.OnPlatformInteract += OnLended;
            _tutorialClickTurn = 1;
            Target.ToTheRightTutorial.SetActive(false);
            Target.ToTheLeftTutorial.SetActive(false);
        }
        
        private void ToTheLeft()
        {
            Target.joystick.Negative.OnClicked -= ToTheLeft;
            Target.joystick.Positive.OnClicked -= ToTheRight;
            Time.timeScale = 1;
            Target.PeanStartTutorial.SetActive(false);
            _tutorialManager.SaveTutorialSteps(TutorialSteps.ToTheMoonFirstJump);
            Target.JumperSoloPlayer.OnPlatformInteract += OnLended;
            _tutorialClickTurn = -1;
            Target.ToTheRightTutorial.SetActive(false);
            Target.ToTheLeftTutorial.SetActive(false);
        }

        private async void OnLended(JumperSoloPlatform platform)
        {
            Target.JumperSoloPlayer.OnPlatformInteract -= OnLended;

            _tutorialManager.SaveTutorialSteps(_tutorialClickTurn > 0
                ? TutorialSteps.ToTheMoonFirstRight
                : TutorialSteps.ToTheMoonFirstLeft);
            await UniTask.Delay(500);

            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.ToTheMoonFirstLeft) ||
                !_tutorialManager.CheckTutorialSteps(TutorialSteps.ToTheMoonFirstRight))
            {
                Time.timeScale = 0;
                if (_tutorialClickTurn > 0)
                {
                    Target.ToTheRightTutorial.SetActive(true);
                    Target.joystick.Negative.OnClicked += ToTheLeft;
                }
                else
                {
                    Target.ToTheLeftTutorial.SetActive(true);
                    Target.joystick.Positive.OnClicked += ToTheRight;
                }
            }
        }
    }
}