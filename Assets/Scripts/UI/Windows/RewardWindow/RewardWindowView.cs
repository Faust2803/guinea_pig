using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Windows.RewardWindow
{
    public class RewardWindowView : BaseWindowView
    {
        [SerializeField] private TextMeshProUGUI  _title;
        [SerializeField] private TextMeshProUGUI  _description;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private TextMeshProUGUI  _yesButtonText;
        [SerializeField] private TextMeshProUGUI  _noButtonText;
        [SerializeField] private Image _hamstaImage;
        [SerializeField] private Image _resourceImage;
        [SerializeField] private TextMeshProUGUI _resourceValue;
        
        protected override void CreateMediator()
        {
            _mediator = new RewardWindowMediator();
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
        
        public Image HamstaImage
        {
            get => _hamstaImage;
            set =>  _hamstaImage = value;
        }
        
        public Image ResourceImage
        {
            get => _resourceImage;
            set =>  _resourceImage = value;
        }
        
        public string ResourceValue
        {
            get => _resourceValue.text;
            set => _resourceValue.text = value;
        }
    }
}