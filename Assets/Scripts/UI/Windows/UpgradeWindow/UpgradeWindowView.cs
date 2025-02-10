
namespace UI.Windows.UpgradeWindow
{
    public class UpgradeWindowView : BaseWindowView
    {
        protected override void CreateMediator()
        {
            _mediator = new UpgradeWindowMediator();
        }
    }
}