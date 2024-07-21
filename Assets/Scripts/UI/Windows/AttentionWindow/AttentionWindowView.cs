using UnityEngine.UI;

namespace UI.Windows.AttentionWindow
{
    public class AttentionWindowView : BaseWindowView
    {
        public Button Home;
        public Button Repeat;

        protected override void CreateMediator()
        {
            _mediator = new AttentionWindowMediator();
        }
    }
}