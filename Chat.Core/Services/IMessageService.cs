using Chat.Core.Models;

namespace Chat.Core.Services
{

    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(string id);
        Task<Message> CreateAsync(MessageCreateDto messageCreateDto);
        Task<AuthResult> DeleteAsync(string id);
    }

}
