
using Chat.Core.Models;

namespace Chat.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserCredentialsDto userRegisterDto);
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RefreshTokenAsync(string token);
    }
}