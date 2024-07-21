
namespace UI.Windows.NewsWindow
{
    public class NewsWindowView : BaseWindowView
    {
        
        protected override void CreateMediator()
        {
            _mediator = new NewsWindowMediator();
        }

        
    }
}