using System;
using System.Threading.Tasks;
using Auth;
using Cysharp.Threading.Tasks;
using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.BootPanel
{
    public class BootPanelMediator :BasePanelMediator <BootPanelView, PanelData>
    {
        [Inject] private AdressableLoaderManager _adressableLoaderManager;
        [Inject] private SceneLoadManagers _sceneLoadManagers;
        [Inject] protected IAuth _auth;
        [Inject] private PlayerManager _playerManager;

        protected override void ShowStart()
        {
            base.ShowStart();
            Target.LoadObject.SetActive(true);
            Target.ProgressText = "0";

            if (_playerManager.DataModel.id == null)
            {
                GoToMain();
            }
            else
            {
                AutarizationCompleat(_playerManager.DataModel.id);
                //GoToLobby();
            }
            
            Target.ErrorMesageText = String.Empty;

            Target.EmailAutchButton.onClick.AddListener(GoToEmailSignIn);
            Target.BacToSignInPanelButton.onClick.AddListener(GoToMain);
            Target.SignUpEmailButton.onClick.AddListener(GoToEmailSignUp);
            Target.BackToSignInButton.onClick.AddListener(GoToEmailSignIn);
            Target.SignInEmailButton.onClick.AddListener(() => EmailSignIn());
            Target.RegistrationEmailButton.onClick.AddListener(() => EmailSignUp());
            Target.GoogleAutchButton.onClick.AddListener(() => GoogleSignIn());
            Target.AppleAutchButton.onClick.AddListener(() => AppleSignIn());
            Target.AnanimusAutchButton.onClick.AddListener(() => AnonymousSignIn());
            Target.SignOutButton.onClick.AddListener(SignOut);
            
            Target.RegistrationEmailInputField.onValueChanged.AddListener(ClearErrorMesage);
            Target.RegistrationPaswordInputField.onValueChanged.AddListener(ClearErrorMesage);
            Target.RegistrationRepeatPaswordInputField.onValueChanged.AddListener(ClearErrorMesage);
            Target.EmailInputField.onValueChanged.AddListener(ClearErrorMesage);
            Target.PaswordInputField.onValueChanged.AddListener(ClearErrorMesage);
            
            _adressableLoaderManager.OnLoadingProgress += LoadAdressabeProgress;
            _adressableLoaderManager.OnLoadingDataComplite += LoadAdressabeCompleat;
            Target.LoadGameButton.onClick.AddListener(GoToLobby);
            _adressableLoaderManager.LoadAdressabless(AssetLabel.Main).Forget();
        }
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            Target.LoadGameButton.onClick.RemoveListener(GoToLobby);
            Target.EmailAutchButton.onClick.RemoveListener(GoToEmailSignIn);
            Target.BacToSignInPanelButton.onClick.RemoveListener(GoToMain);
            Target.SignUpEmailButton.onClick.RemoveListener(GoToEmailSignUp);
            Target.BackToSignInButton.onClick.RemoveListener(GoToEmailSignIn);
            Target.SignInEmailButton.onClick.RemoveListener(() => EmailSignIn());
            Target.RegistrationEmailButton.onClick.RemoveListener(() => EmailSignUp());
            Target.GoogleAutchButton.onClick.RemoveListener(() => GoogleSignIn());
            Target.AppleAutchButton.onClick.RemoveListener(() => AppleSignIn());
            Target.AnanimusAutchButton.onClick.RemoveListener(() => AnonymousSignIn());
            Target.SignOutButton.onClick.RemoveListener(SignOut);
            
            Target.RegistrationEmailInputField.onValueChanged.RemoveListener(ClearErrorMesage);
            Target.RegistrationPaswordInputField.onValueChanged.RemoveListener(ClearErrorMesage);
            Target.RegistrationRepeatPaswordInputField.onValueChanged.RemoveListener(ClearErrorMesage);
            Target.EmailInputField.onValueChanged.RemoveListener(ClearErrorMesage);
            Target.PaswordInputField.onValueChanged.RemoveListener(ClearErrorMesage);
        }
        
        private void LoadAdressabeCompleat()
        {
            Debug.Log("BootPanelMediator LoadAdressabeCompleat");
            
            _adressableLoaderManager.OnLoadingDataComplite -= LoadAdressabeCompleat;
            _adressableLoaderManager.OnLoadingProgress -= LoadAdressabeProgress;

            //GoToLobby();
            
            Target.LoadObject.SetActive(false);
        }
        
        private void LoadAdressabeProgress(float persent, float loaded, float all)
        {
            Debug.Log($"BootPanelMediator LoadAdressabeProgress progress = {persent}");
            Target.ProgressText = $"Loading {persent}% ({loaded}/{all} MB)";
        }

        private void GoToLobby()
        {
            CloseSelf();
            _sceneLoadManagers.LoadScene(Scene.Lobby);
            
        }

        private void GoToEmailSignIn()
        {
            Target.SignInPanel.SetActive(true);
            Target.RegistrationPanel.SetActive(false);
            Target.AuthPanel.SetActive(false);
            
            Target.EmailInputFieldText = String.Empty;
            Target.PaswordInputFieldText = String.Empty;
            
            ErrorMesage();
        }
        
        private void GoToEmailSignUp()
        {
            Target.SignInPanel.SetActive(false);
            Target.RegistrationPanel.SetActive(true);
            Target.AuthPanel.SetActive(false);
            
            Target.RegistrationEmailInputFieldText = String.Empty;
            Target.RegistrationPaswordInputFieldText = String.Empty;
            Target.RegistrationRepeatPaswordInputFieldText = String.Empty;
            
            ErrorMesage();
        }
        
        private void GoToMain()
        {
            Target.SignInPanel.SetActive(false);
            Target.RegistrationPanel.SetActive(false);
            Target.AuthPanel.SetActive(true);
            ErrorMesage();
            
            Target.LoadGameButton.gameObject.SetActive(false);
            Target.SignOutButton.gameObject.SetActive(false);
        }
        
        private void AutarizationCompleat(string userId)
        {
            Target.SignInPanel.SetActive(false);
            Target.RegistrationPanel.SetActive(false);
            Target.AuthPanel.SetActive(false);
            ErrorMesage();
            
            _playerManager.OnDataLoaded += GoToLobby;
            _playerManager.OnDataSaved += GoToLobby;
            _playerManager.SignIn(userId);
        }

        private void GoToLobby(bool result)
        {
            //Debug.Log($"BootPanelMediator GoToLobby {result}");
            if (result)
            {
                Target.LoadGameButton.gameObject.SetActive(true);
                Target.SignOutButton.gameObject.SetActive(true);
                _playerManager.OnDataLoaded -= GoToLobby;
                _playerManager.OnDataSaved -= GoToLobby;
                GoToLobby();
            }
        }

        private async Task EmailSignIn()
        {
            if (Target.EmailInputFieldText == string.Empty || Target.PaswordInputFieldText == string.Empty)
            {
                ErrorMesage(_auth.ErrorType(AuthErrorType.NotBeEmpty));
                return;
            }
            var result = await _auth.Login(AuthType.Email, Target.EmailInputFieldText, Target.PaswordInputFieldText);
            if (result.Success)
            {
                AutarizationCompleat(result.UserId);
            }
            else
            {
                ErrorMesage(result.Message);
            }
        }
        
        private async Task EmailSignUp()
        {
            if (Target.RegistrationPaswordInputFieldText != Target.RegistrationRepeatPaswordInputFieldText)
            {
                ErrorMesage(_auth.ErrorType(AuthErrorType.DontMatch));
                return;
            }
            if (Target.RegistrationPaswordInputFieldText == string.Empty || Target.RegistrationEmailInputFieldText == string.Empty)
            {
                ErrorMesage(_auth.ErrorType(AuthErrorType.NotBeEmpty));
                return;
            }
            var result = await _auth.Registration(Target.RegistrationEmailInputFieldText, Target.RegistrationPaswordInputFieldText);
            if (result.Success)
            {
                AutarizationCompleat(result.UserId);
            }
            else
            {
                ErrorMesage(result.Message);
            }
        }
        
        private async Task AnonymousSignIn()
        {
            var result = await _auth.Login(AuthType.Anonymous);
            if (result.Success)
            {
                AutarizationCompleat(result.UserId);
            }
            else
            {
                ErrorMesage(result.Message);
            }
        }
        
        private async Task GoogleSignIn()
        {
            var result = await _auth.Login(AuthType.Google);
            if (result.Success)
            {
                AutarizationCompleat(result.UserId);
            }
            else
            {
                ErrorMesage(result.Message);
            }
        }
        
        private async Task AppleSignIn()
        {
            var result = await _auth.Login(AuthType.Apple);
            if (result.Success)
            {
                AutarizationCompleat(result.UserId);
            }
            else
            {
                ErrorMesage(result.Message);
            }
        }
        
        private void ErrorMesage(string message = "")
        {
            Target.ErrorMesageText = message;
        }
        
        private void ClearErrorMesage(string message)
        {
            ErrorMesage();
        }
        
        private void SignOut()
        {
            _auth.SignOut();
            GoToMain();
            _playerManager.SignOut();
        }
    }
}