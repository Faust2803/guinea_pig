
namespace UI.Windows.ShopWindow
{
    public class ShopWindowView : BaseWindowView
    {
        
        protected override void CreateMediator()
        {
            _mediator = new ShopWindowMediator();
        }
    }
}