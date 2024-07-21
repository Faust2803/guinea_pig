using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.PyramidWindow
{
    public class PyramidWindowView : BaseWindowView
    {
        public CanvasGroup CanvasGroup;
        public Image Background;
        public Ease EaseShowUi = Ease.OutSine;
        public Ease EaseHideUi = Ease.InSine;
        public Ease EaseShowBg = Ease.OutSine;
        public Ease EaseHideBg = Ease.InSine;

        public ScrollPyramidButton ToMoonScrollBtn;
        public ScrollPyramidButton ToHamsterScrollBtn;
        
        protected override void CreateMediator()
        {
            _mediator = new PyramidWindowMediator();
        }

        public void Show(float duration)
        {
            CanvasGroup.DOFade(1f, duration).SetEase(EaseShowUi).OnComplete((() => CanvasGroup.interactable = true));
            Background.DOFade(0f, duration).SetEase(EaseShowBg);
        }

        public void Hide(float duration)
        {
            CanvasGroup.DOFade(0f, duration).SetEase(EaseHideUi).OnComplete((() => CanvasGroup.interactable = true));
            Background.DOFade(1f, duration).SetEase(EaseHideBg);
        }
    }
}