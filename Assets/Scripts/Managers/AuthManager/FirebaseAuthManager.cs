using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

namespace Auth
{
    public class FirebaseAuthManager : IAuth
    {
        private FirebaseAuth _auth;
        
        public async Task Init()
        {
            // var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            // if (dependencyStatus == DependencyStatus.Available)
            // {
            //     _auth = FirebaseAuth.DefaultInstance;
            //     Debug.Log("Firebase инициализирован");
            // }
            // else
            // {
            //     Debug.LogError($"Firebase не инициализирован: {dependencyStatus}");
            // }
            _auth = FirebaseAuth.DefaultInstance;
        }
        
        public async Task<AuthResult> Login(AuthType type, string username = "", string password =  "")
        {
            Debug.Log($"Login type = {type}, username = {username}, password = {password}");
            AuthResult result = null;
            switch (type)
            {
                case AuthType.Anonymous:
                    result = await SignInAnonymouslyAsync();
                    break;
                case AuthType.Email:
                    result = await SignInWithEmailAsync(username, password);
                    break;
                case AuthType.Google:
                    result = await SignInWithGoogleAsync();
                    break;
                case AuthType.Apple:
                    result = await SignInWithAppleAsync();
                    break;
                case AuthType.Phone:
                    result = await SignInWithPhoneAsync(username);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return result;
        }

        public async Task<AuthResult> Registration(string email, string password)
        {
            if (_auth == null)
            {
                Debug.LogError("FirebaseAuth не инициализирован");
                return null;
            }
            try
            {
                var userCredential = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                //Debug.Log($"Анонимный вход выполнен! UserID: {userCredential.User.UserId}");
                return new AuthResult(true, userCredential.User.UserId);
            }
            catch (FirebaseException e)
            {
                //Debug.LogError($"Ошибка анонимного входа: {e.Message}");
                
                return new AuthResult(false, null, ErrorType((AuthErrorType)e.ErrorCode));
            }
        }

        public void SignOut()
        {
            _auth.SignOut();
        }
        
        private async Task<AuthResult> SignInAnonymouslyAsync()
        {
            if (_auth == null)
            {
                Debug.LogError("FirebaseAuth не инициализирован");
                return null;
            }
            try
            {
                var userCredential = await _auth.SignInAnonymouslyAsync();
                //Debug.Log($"Анонимный вход выполнен! UserID: {userCredential.User.UserId}");
                return new AuthResult(true, userCredential.User.UserId);
            }
            catch (FirebaseException e)
            {
                //Debug.LogError($"Ошибка анонимного входа: {e.Message}");
                return new AuthResult(false, null, ErrorType((AuthErrorType)e.ErrorCode));
            }
        }
        
        private async Task<AuthResult> SignInWithEmailAsync(string email, string password)
        {
            if (_auth == null)
            {
                Debug.LogError("FirebaseAuth не инициализирован");
                return null;
            }
            try
            {
                var userCredential = await _auth.SignInWithEmailAndPasswordAsync( email, password);
                //Debug.Log($"Анонимный вход выполнен! UserID: {userCredential.User.UserId}");
                return new AuthResult(true, userCredential.User.UserId);
            }
            catch (FirebaseException e)
            {
                //Debug.LogError($"Ошибка анонимного входа: {e.Message}");
                
                return new AuthResult(false, null, ErrorType((AuthErrorType)e.ErrorCode));
            }
        }
        
        private async Task<AuthResult> SignInWithGoogleAsync()
        {
            return new AuthResult(false, null, null);
        }
        
        private async Task<AuthResult> SignInWithAppleAsync()
        {
            return new AuthResult(false, null,null);
        }
        
        private async Task<AuthResult> SignInWithPhoneAsync(string username)
        {
            return new AuthResult(false, null, null);
        }

        public string ErrorType(AuthErrorType errorCode)
        {
            switch (errorCode)
            {
                case AuthErrorType.NoSuchUser:
                    return "No such user found";
                case AuthErrorType.BadEmailOrPassword:
                    return "Authorization error";
                case AuthErrorType.BadRepassword:
                    return "Authorization error";
                case AuthErrorType.RegistrationError:
                    return "Authorization error";
                case AuthErrorType.AuthenticationError:
                    return "Authorization error";
                case AuthErrorType.DontMatch:
                    return "A Passwords and repeat password don't match!";
                case AuthErrorType.NotBeEmpty:
                    return "Email and password must not be empty";
                default:
                    return "Authorization error";
            }
        }
        
    }
    
    public class AuthResult
    {
        public bool Success { get; }
        public string UserId { get; }
        public string Message { get; }

        public AuthResult(bool success, string userId, string message = null)
        {
            Success = success;
            UserId = userId;
            Message = message;
        }
    }
}