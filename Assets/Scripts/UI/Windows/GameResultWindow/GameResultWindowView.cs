using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Windows.GameResultWindow
{
    public class GameResultWindowView : BaseWindowView
    {
        protected override void CreateMediator()
        {
            _mediator = new GameResultWindowMediator();
        }
        
    }
}