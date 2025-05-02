
using Chat.Core.Models;

namespace Chat.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> Register(UserCredentialsDto userRegisterDto);
        Task<AuthResult> Login(string username, string password);

    }
}