
namespace UI.Windows.SettingsWindow
{
    public class SettingsWindowView : BaseWindowView
    {
       
        protected override void CreateMediator()
        {
            _mediator = new SettingsWindowMediator();
        }
    }
}