
using Chat.Core.Models;

namespace Chat.Core.Services
{
    public interface IUsersService
    {
        Task<AuthResult> RegisterAsync(UserCredentialsDto userRegisterDto);
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RefreshTokenAsync(string userName);
        Task<User> GetByUserNameAsync(string userName);
    }
}