using UI;
using UnityEngine;

namespace UI.Panels
{
    public abstract class BasePanelView : BaseView
    {

        protected BasePanelMediator _mediator;

        public BasePanelMediator BaseMediator => _mediator;

        public GameObject Panel => this.gameObject;

        public void OnCreateMediator(out BasePanelMediator mediator)
        {
            mediator = _mediator;
        }
        
        
        public override void Init()
        {
            base.Init();
            _mediator.Mediate(this);
        }
    }
}