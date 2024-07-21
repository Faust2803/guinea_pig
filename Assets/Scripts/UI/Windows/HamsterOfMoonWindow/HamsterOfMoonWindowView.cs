using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.HamsterOfMoonWindow
{
    public class HamsterOfMoonWindowView : BaseWindowView
    {
        [Header("References")]
        [SerializeField] private Image _bg;
        [SerializeField] private RectTransform _parentTr;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _jackpotValueText;
        [SerializeField] private CanvasGroup _canvasGroupParticles;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _defaultIcon;
        
        [Header("Animation parameters")]
        [SerializeField] private float _durationShow;
        [SerializeField] private float _durationHide;
        [SerializeField] private Ease _easeShowPopup;
        [SerializeField] private Ease _easeHidePopup;
        [SerializeField] private Ease _easeShowBg;
        [SerializeField] private Ease _easeHideBg;
        [SerializeField, Range(0f, 1f)] private float _targetBgAlpha;

        private Sequence _sequence;
        
        protected override void CreateMediator()
        {
            _mediator = new HamsterOfMoonWindowMediator();
        }

        public void Initialize(string playerName, int value, Sprite icon = null)
        {
            _icon.sprite = icon == null ? _defaultIcon : icon;
            _playerNameText.text = playerName;
            _jackpotValueText.text = value.ToString();
        }

        public void Show(Action callBack = null)
        {
            SetStartValue();
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            var alphaTween = _bg.DOFade(_targetBgAlpha, _durationShow).SetEase(_easeShowBg);
            var popupTween = _parentTr.DOScale(Vector3.one, _durationShow).SetEase(_easeShowPopup);

            _sequence.Append(alphaTween).Join(popupTween).AppendCallback(() => callBack?.Invoke());
        }

        public void Hide(Action callBack = null)
        {
            SetEndValue();
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            var alphaTween = _bg.DOFade(0f, _durationHide).SetEase(_easeHideBg);
            var popupTween = _parentTr.DOScale(Vector3.zero, _durationHide).SetEase(_easeHidePopup);
            var particleFadeTween = _canvasGroupParticles.DOFade(0f, _durationHide).SetEase(_easeHideBg);
            _sequence.Append(alphaTween).Join(popupTween).Join(particleFadeTween).AppendCallback(() => callBack?.Invoke());
        }

        public void SetStartValue()
        {
            var color = _bg.color;
            color.a = 0f;
            _bg.color = color;

            _parentTr.localScale = Vector3.zero;
            _canvasGroupParticles.alpha = 1f;
        }

        public void SetEndValue()
        {
            var color = _bg.color;
            color.a = _targetBgAlpha;
            _bg.color = color;

            _parentTr.localScale = Vector3.one;
            _canvasGroupParticles.alpha = 0f;
        }

        [ContextMenu(nameof(ShowTest))]
        private void ShowTest()
        {
            Show();
        }

        [ContextMenu(nameof(HideTest))]
        private void HideTest()
        {
            Hide();
        }
    }
}