using Chat.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> GetByIdAsync(string id);
        Task<IEnumerable<Message>> GetAllAsync();
        Task CreateAsync(Message message);
        Task DeleteAsync(string id);
    }
}
