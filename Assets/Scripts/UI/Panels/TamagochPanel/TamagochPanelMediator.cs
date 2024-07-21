using DataModels.PlayerData;
using Managers;
using Managers.Analytics;
using Managers.HamsterPreviewManager;
using Managers.SoundManager.Base;
using UI.Windows;
using UI.Windows.RewardWindow;
using Zenject;

namespace UI.Panels.TamagochWindow
{
    public class TamagochPanelMediator : BasePanelMediator<TamagochPanelView, PanelData>
    {
        [Inject] private ISoundManager _sounds;
        [Inject] private PlayerManager _playerManager;
        [Inject] private HamsterPreviewManager _hamsterPreviewManager; 
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private TutorialManager _tutorialManager;

        protected override void ShowStart()
        {
            base.ShowStart();
            _playerManager.Resources[ResourcesType.GoldenBean].SetNewValue += UpdateResource;
            UpdateResource(
                _playerManager.Resources[ResourcesType.GoldenBean].Value,
                _playerManager.Resources[ResourcesType.GoldenBean].ResourceType);

            Target.FeedHamsterButton.onClick.AddListener(OnFeedHamsterButton);
            Target.CleanHamsterButton.onClick.AddListener(OnBrushHamsterButton);
            Target.DanceHamsterButton.onClick.AddListener(OnDanceHamsterButton);
            Target.PlayWithHamsterButton.onClick.AddListener(OnPlayWithHamsterButton);

            _hamsterPreviewManager.OnHamsterEnteredIdleState += OnHamsterEnteredIdleState;
            _hamsterPreviewManager.ShowPreview(true);

            Target.HamsterPreviewView.UpdatePreviewAndHPPreview(_playerManager.CurrentCollectionItem);

            if (_playerManager.CurrentCollectionItem.collection_current_hp < 2)
            {
                _hamsterPreviewManager.PlaySadAnimation();
                Target.SetSadBackground();
            }
            
            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.GetFirstHamsta))
            {
                StartTutorial();
            }
            // if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.GameFlow))
            // {
            //     ProgressTutorialStep();
            // }
            Target.NicknameText.text = _playerManager.PlayerName;
            Target.AnalyticsButton.onClick.AddListener(OnHamstaTap);

        }

        protected override void CloseStart()
        { 
            base.CloseStart();

            Target.FeedHamsterButton.onClick.RemoveListener(OnFeedHamsterButton);
            Target.CleanHamsterButton.onClick.RemoveListener(OnBrushHamsterButton);
            Target.DanceHamsterButton.onClick.RemoveListener(OnDanceHamsterButton);
            Target.PlayWithHamsterButton.onClick.RemoveListener(OnPlayWithHamsterButton);

            _hamsterPreviewManager.ShowPreview(false);
            _hamsterPreviewManager.OnHamsterEnteredIdleState -= OnHamsterEnteredIdleState;
            
            Target.AnalyticsButton.onClick.RemoveListener(OnHamstaTap);
        }

        private void OnHamsterEnteredIdleState()
        {
            Target.ResetBackgroundColor();
        }

        private async void OnFeedHamsterButton()
        {
           /*
            * FeedHamster(int hp = 1, int id = -1)
            * hp - кол-во hp на которое кормим хомяка
            * id - какой именно єлемент коллекции (хомяка кормим),  по умолчанию -1 - кормим хомякак который сейчас применен на плеере
            * возвращает bool true -если операция прошла успешно false - если провал
            */
            var result = await _playerManager.FeedHamster();

            if (result)
            {
                Target.HamsterPreviewView.UpdatePreviewAndHPPreview(_playerManager.CurrentCollectionItem);
                Target.SetFeedBackground();
                _hamsterPreviewManager.PlayFeedAnimation();
                _sounds.PlayOneShot(Managers.SoundManager.Enums.SoundId.ChompTamagoch);
                
                _analyticsManager.Tamagotchi("fed_hamster"); 
            }
        }

        private void UpdateResource(int value, ResourcesType type)
        {
            Target.CurrencyItem.SetCurrencyData(value, type);
        }

        private void OnPlayWithHamsterButton()
        {
            _hamsterPreviewManager.PlayPlayWithAnimation();
            _sounds.PlayOneShot(Managers.SoundManager.Enums.SoundId.CuteTamagoch);
            Target.SetPlayWithBackground();
            
            _analyticsManager.Tamagotchi("brushed_hamster");
        }

        private void OnDanceHamsterButton()
        {
            _hamsterPreviewManager.PlayDiscoAnimation();
            Target.SetDanceBackground();
            
            _analyticsManager.Tamagotchi("dance_hamster");
        }

        private void OnBrushHamsterButton()
        {
            _hamsterPreviewManager.PlayBrushAnimation();
            _sounds.PlayOneShot(Managers.SoundManager.Enums.SoundId.Scratchham);
            Target.SetCleanBackground();
        }
        
        private void StartTutorial()
        {
            _hamsterPreviewManager.PlayFirstEnterAnimation();
            Target.ButtonPanel.SetActive(false);
            Target.NicknameText.gameObject.SetActive(false);
            Target.Locker.SetActive(true);
            Target.StartScreenPanel.SetActive(true);
            Target.ClameButton.onClick.AddListener(ProgressTutorialStep);
            _analyticsManager.TutorialFirstScreen();
            _tutorialManager.SaveTutorialSteps(TutorialSteps.GetFirstHamsta);
        }

        private void ProgressTutorialStep()
        {
            _hamsterPreviewManager.PlayIdleAnimation();
            Target.NicknameText.gameObject.SetActive(true);
            Target.ButtonPanel.SetActive(true);
            Target.ClameButton.onClick.RemoveListener(ProgressTutorialStep);
            Target.Locker.SetActive(false);
            Target.StartScreenPanel.SetActive(false);
            
            if (!_tutorialManager.CheckTutorialSteps(TutorialSteps.GameFlow))
            {
                _analyticsManager.TutorialSecondScreen();
                _uiManager.OpenWindow(WindowType.HowToProgressTutorialWindow);
                _tutorialManager.SaveTutorialSteps(TutorialSteps.GameFlow);
            }
            
        }

        private void OnHamstaTap()
        {
            _analyticsManager.Tamagotchi("tap_on_hamster"); 
        }
    }
}