using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.PyramidWindow
{
    public class ScrollPyramidButton : MonoBehaviour
    {
        public event Action OnClick;
        public bool IsVisible => _isVisible;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _btn;
        [SerializeField] private float _durationShow = 0.5f;
        [SerializeField] private Ease _easeShow = Ease.OutSine;
        [SerializeField] private float _durationHide = 0.5f;
        [SerializeField] private Ease _easeHide = Ease.InSine;

        private Func<bool> _visibleCondition;
        private bool _isInitialized;
        private bool _isHided;
        private bool _isVisible;
        private Tweener _tweener;

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnClicked);
        }

        public void Initialize(Func<bool> predicate)
        {
            _isInitialized = true;
            _visibleCondition = predicate;
        }

        public void Deinitialize()
        {
            _isInitialized = false;
            _visibleCondition = null;
            _isHided = false;
            _isVisible = false;
        }

        public void Show()
        {
            _isHided = false;
        }

        public void Hide()
        {
            _isHided = true;
            
            if (_isVisible)
            {
                _isVisible = false;
                HideAnimation();
            }
        }

        private void OnClicked()
        {
            OnClick?.Invoke();
        }

        private void Update()
        {
            if(!_isInitialized || _isHided) return;

            var isVisible = _visibleCondition.Invoke();
            
            if (isVisible == _isVisible) return;

            _isVisible = isVisible;
            
            if (_isVisible)
                ShowAnimation();
            else
                HideAnimation();
        }

        private void ShowAnimation()
        {
            _btn.interactable = true;
            _tweener?.Kill();
            _tweener = _canvasGroup.DOFade(1f, _durationShow * (1 - _canvasGroup.alpha)).SetEase(_easeShow);
        }

        private void HideAnimation()
        {
            _btn.interactable = false;
            _tweener?.Kill();
            _tweener = _canvasGroup.DOFade(0f, _durationHide * _canvasGroup.alpha).SetEase(_easeHide);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveListener(OnClicked);
            _tweener?.Kill();
            _canvasGroup.alpha = 0f;
        }
    }
}