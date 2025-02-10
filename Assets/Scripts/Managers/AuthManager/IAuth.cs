using System;
using System.Threading.Tasks;

namespace Auth
{
    public interface IAuth
    {
        public Task Init();
        public  Task<AuthResult> Login(AuthType type, string username = "", string password = "");
        public Task<AuthResult> Registration(string username, string password);
        public void SignOut();
        public string ErrorType(AuthErrorType errorCode);

    }
}