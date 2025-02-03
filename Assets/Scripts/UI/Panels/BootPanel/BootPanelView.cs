using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BootPanel
{
    public class BootPanelView : BasePanelView
    {
        [SerializeField] private Button  _loadGameButton;
        [SerializeField] private GameObject  _loadObject;
        [SerializeField] private TextMeshProUGUI  _progressText;
        
        protected override void CreateMediator()
        {
            _mediator = new BootPanelMediator();
        }

        public Button LoadGameButton => _loadGameButton;
        
        public GameObject LoadObject => _loadObject;

        public string ProgressText
        {
            get => _progressText.text;
            set => _progressText.text = value;
        }

       
        
    }
}