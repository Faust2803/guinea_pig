using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BootPanel
{
    public class BottomPanelView : BasePanelView
    {

        [SerializeField] private Button  _playWindowButton;
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new BottomPanelMediator();
        }
        
        public Button PlayWindowButton => _playWindowButton;
        
    }
}