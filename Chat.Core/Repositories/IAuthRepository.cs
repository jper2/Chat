using Chat.Core.Models;

namespace Chat.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }

}
