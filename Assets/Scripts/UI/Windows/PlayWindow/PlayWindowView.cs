
namespace UI.Windows.PlayWindow
{
    public class PlayWindowView : BaseWindowView
    {
        
        protected override void CreateMediator()
        {
            _mediator = new PlayWindowMediator();
        }

        
    }
}