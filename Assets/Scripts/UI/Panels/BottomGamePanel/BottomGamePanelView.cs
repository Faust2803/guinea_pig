using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BottomGamePanel
{
    public class BottomGamePanelView : BasePanelView
    {
        [SerializeField] private Button  _fireButton;
        [SerializeField] private Button  _reloadButton;
       
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new BottomGamePanelMediator();
        }

        public Button FireButton => _fireButton;
        public Button ReloadButton => _reloadButton;

    }
}