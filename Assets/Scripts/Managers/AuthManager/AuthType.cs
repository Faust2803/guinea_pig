namespace Auth
{
        public enum AuthType
        {
            Anonymous,
            Email,
            Google,
            Apple,
            Phone
        }
        
        public enum AuthErrorType
        {
            BadEmailOrPassword,
            NoSuchUser = 1,
            BadRepassword,
            RegistrationError,
            AuthenticationError = 100,
            DontMatch = 101,
            NotBeEmpty = 102,
            
        }
}