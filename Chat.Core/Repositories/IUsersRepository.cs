using Chat.Core.Models;

namespace Chat.Core.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }

}
