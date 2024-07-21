
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Tutorial
{
    public class HowToProgressTutorialView : BaseWindowView
    {
        
        
        protected override void CreateMediator()
        {
            _mediator = new HowToProgressTutorialMediator();
        }
    }
}