using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BottomGamePanel
{
    public class BottomGamePanelView : BasePanelView
    {
        [SerializeField] private Button  _fireButton;
        [SerializeField] private Button  _reloadButton;
        [SerializeField] private Image  _fireButtonLockImage;
       
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new BottomGamePanelMediator();
        }

        public Button FireButton => _fireButton;
        public GameObject FireButtonLockImage => _fireButtonLockImage.gameObject;
        public Button ReloadButton => _reloadButton;

    }
}