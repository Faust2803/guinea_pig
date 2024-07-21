using System.Collections.Generic;
using DataModels.CollectionsData;
using UI.Panels.ToplobbyPanel;
using UI.Views;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Windows.TamagochWindow
{
    public class TamagochWindowView: BaseWindowView
    {
        [SerializeField] private Button _feedHamsterButton;
        [SerializeField] private Button _danceHamsterButton;
        [SerializeField] private Button _cleanHamsterButton;
        [SerializeField] private Button _playWithHamsterButton;
        [SerializeField] private CurrencyItem _currencyItem;
        [SerializeField] private HamsterPreviewViewBase _hamsterPreviewView;

        [Header("Background settings")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _sadColor;
        [SerializeField] private Color _feedColor;
        [SerializeField] private Color _danceColor;
        [SerializeField] private Color _playWithColor;
        [SerializeField] private float _colorChangeDuration = 0.5f;
        private Color _currentBackgroundColor;

        public CurrencyItem CurrencyItem => _currencyItem;
        public HamsterPreviewViewBase HamsterPreviewView => _hamsterPreviewView;

        protected override void CreateMediator()
        {
            _mediator = new TamagochWindowMediator();

            _currentBackgroundColor = _feedColor;
        }

        public Button FeedHamsterButton => _feedHamsterButton;
        public Button DanceHamsterButton => _danceHamsterButton;
        public Button CleanHamsterButton => _cleanHamsterButton;
        public Button PlayWithHamsterButton => _playWithHamsterButton;

        public void SetSadBackground() => SetBackgroundDefaultColor(_sadColor);
        public void SetPlayWithBackground() => AnimateBackgroundColor(_playWithColor);
        public void SetDanceBackground() => AnimateBackgroundColor(_danceColor);
        public void SetCleanBackground() => AnimateBackgroundColor(_defaultColor);
        public void SetFeedBackground()
        {
            if (!_currentBackgroundColor.Equals(_feedColor))
                AnimateBackgroundColor(_feedColor);

            SetBackgroundDefaultColor(_feedColor);
        }

        public void ResetBackgroundColor() => AnimateBackgroundColor(_currentBackgroundColor);

        private void SetBackgroundDefaultColor(Color color)
        {
            _backgroundImage.color = color;
            _currentBackgroundColor = color;
        }

        private void AnimateBackgroundColor(Color color)
        {
            _backgroundImage.DOColor(color, _colorChangeDuration);
        }
    }
}