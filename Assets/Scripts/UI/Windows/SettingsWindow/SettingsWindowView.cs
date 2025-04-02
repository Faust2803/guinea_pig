
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.SettingsWindow
{
    public class SettingsWindowView : BaseWindowView
    {
        [SerializeField] private Button _signOutButton;
        
        public Button SignOutButton => _signOutButton;
       
        protected override void CreateMediator()
        {
            _mediator = new SettingsWindowMediator();
        }
    }
}