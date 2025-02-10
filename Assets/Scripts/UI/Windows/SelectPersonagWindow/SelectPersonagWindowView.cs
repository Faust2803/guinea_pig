using UnityEngine.UI;

namespace UI.Windows.SelectPersonag
{
    public class SelectPersonagWindowView : BaseWindowView
    {
        protected override void CreateMediator()
        {
            _mediator = new SelectPersonagWindowMediator();
        }
    }
}