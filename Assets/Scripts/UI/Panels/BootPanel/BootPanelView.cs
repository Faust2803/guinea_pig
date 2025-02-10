using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BootPanel
{
    public class BootPanelView : BasePanelView
    {
        [SerializeField] private Button  _loadGameButton;
        
        [Header("Login Panels")]
        [SerializeField] private GameObject  _signInPanel;
        [SerializeField] private GameObject  _registrationPanel;
        [SerializeField] private GameObject  _authPanel;
        
        [Header("Login Button")]
        [SerializeField] private Button  _ananimusAutchButton;
        [SerializeField] private Button  _emailAutchButton;
        [SerializeField] private Button  _googleAutchButton;
        [SerializeField] private Button  _appleAutchButton;
        
        [Header("SignIn Email Panel Elements")]
        [SerializeField] private InputField  _emailInputField;
        [SerializeField] private InputField  _paswordInputField;
        [SerializeField] private Button  _signInEmailButton;
        [SerializeField] private Button  _signUpEmailButton;
        [SerializeField] private Button  _bacToSignInPanelButton;
        
        [Header("SignUp Email Panel Elements")]
        [SerializeField] private InputField  _registrationEmailInputField;
        [SerializeField] private InputField  _registrationPaswordInputField;
        [SerializeField] private InputField  _registrationRepeatPaswordInputField;
        [SerializeField] private Button  _registrationEmailButton;
        [SerializeField] private Button  _backToSignInButton;
        
        [Header("Loading Elements")]
        [SerializeField] private GameObject  _loadObject;
        [SerializeField] private TextMeshProUGUI  _progressText;
        
        [Space]
        [SerializeField] private TextMeshProUGUI  _errorMesageText;
        [Space]
        [SerializeField] private Button  _signOutButton;
        protected override void CreateMediator()
        {
            _mediator = new BootPanelMediator();
        }

        public Button LoadGameButton => _loadGameButton;
        public Button AnanimusAutchButton => _ananimusAutchButton;
        public Button EmailAutchButton => _emailAutchButton;
        public Button GoogleAutchButton => _googleAutchButton;
        public Button AppleAutchButton => _appleAutchButton;
        public GameObject SignInPanel => _signInPanel;
        public GameObject RegistrationPanel => _registrationPanel;
        public GameObject AuthPanel => _authPanel;
        public InputField  EmailInputField => _emailInputField;
        public InputField PaswordInputField => _paswordInputField;
        public Button SignInEmailButton => _signInEmailButton;
        public Button SignUpEmailButton => _signUpEmailButton;
        public InputField RegistrationEmailInputField => _registrationEmailInputField;
        public InputField RegistrationPaswordInputField => _registrationPaswordInputField;
        public InputField RegistrationRepeatPaswordInputField => _registrationRepeatPaswordInputField;
        public Button RegistrationEmailButton => _registrationEmailButton;
        public Button BackToSignInButton => _backToSignInButton;
        public Button BacToSignInPanelButton => _bacToSignInPanelButton;
        public Button SignOutButton => _signOutButton;
        
        public GameObject LoadObject => _loadObject;

        public string ProgressText
        {
            get => _progressText.text;
            set => _progressText.text = value;
        }
        
        public string RegistrationEmailInputFieldText
        {
            get => _registrationEmailInputField.text;
            set => _registrationEmailInputField.text = value;
        }
        
        public string RegistrationPaswordInputFieldText
        {
            get => _registrationPaswordInputField.text;
            set => _registrationPaswordInputField.text = value;
        }
        
        public string RegistrationRepeatPaswordInputFieldText
        {
            get => _registrationRepeatPaswordInputField.text;
            set => _registrationRepeatPaswordInputField.text = value;
        }
        
        public string EmailInputFieldText
        {
            get => _emailInputField.text;
            set => _emailInputField.text = value;
        }
        
        public string PaswordInputFieldText
        {
            get => _paswordInputField.text;
            set => _paswordInputField.text = value;
        }
        
        public string ErrorMesageText
        {
            get => _errorMesageText.text;
            set => _errorMesageText.text = value;
        }
    }
}