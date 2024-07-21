using System.Collections.Generic;
using DataModels.Achievement;
using DataModels.PlayerData;
using Managers;
using Managers.HamsterPreviewManager;
using Managers.SoundManager.Base;
using Zenject;

namespace UI.Windows.TamagochWindow
{
    public class TamagochWindowMediator : BaseWindowMediator<TamagochWindowView, WindowData>
    {
        [Inject] private ISoundManager _sounds;
        [Inject] private PlayerManager _playerManager;
        [Inject] private HamsterPreviewManager _hamsterPreviewManager;
        [Inject] private AchievementManager _achievementManager;
        
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
                
                var achievementTargets = new List<AchievementTarget>();
                achievementTargets.Add(new AchievementTarget(TargetType.FeedHamster, 1));
                achievementTargets.Add(new AchievementTarget(TargetType.SpendBeans, PlayerManager.Feed_Hamster_exchange_Rate));
                _achievementManager.CheckAchievementTarget(achievementTargets.ToArray());
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
        }

        private void OnDanceHamsterButton()
        {
            _hamsterPreviewManager.PlayDiscoAnimation();
            Target.SetDanceBackground();
        }

        private void OnBrushHamsterButton()
        {
            _hamsterPreviewManager.PlayBrushAnimation();
            _sounds.PlaySound(Managers.SoundManager.Enums.SoundId.Scratchham, restart: true);
            Target.SetCleanBackground();
        }

    }
}