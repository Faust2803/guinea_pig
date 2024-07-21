using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.SimpleDialogWindow
{
    public class SimpleDialogWindowView : BaseWindowView
    {
        [SerializeField] private TextMeshProUGUI  _title;
        [SerializeField] private TextMeshProUGUI  _description;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private TextMeshProUGUI  _yesButtonText;
        [SerializeField] private TextMeshProUGUI  _noButtonText;
        [SerializeField] private Image _image;
        
        
        protected override void CreateMediator()
        {
            _mediator = new SimpleDialogWindowMediator();
        }
        
        public Button YesButton => _yesButton;

        public Button NoButton => _noButton;

        public string Description
        {
            get => _description.text;
            set => _description.text = value;
        }
        
        public string Title
        {
            get => _title.text;
            set => _title.text = value;
        }
        
        public string YesButtonText
        {
            get => _yesButtonText.text;
            set => _yesButtonText.text = value;
        }
        
        public string NoButtonText
        {
            get => _noButtonText.text;
            set => _noButtonText.text = value;
        }
        
        public Image Image
        {
            get => _image;
            set =>  _image = value;
        }
    }
}