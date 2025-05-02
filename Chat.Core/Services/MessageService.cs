using Chat.Core.Models;
using Chat.Core.Repositories;

namespace Chat.Core.Services
{

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<Message> GetByIdAsync(string id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task<Message> CreateAsync(MessageCreateDto messageCreateDto)
        {
            var message = new Message
            {
                Content = messageCreateDto.Content,
                Type = messageCreateDto.Type,
                CreatedAt = DateTime.UtcNow,
                UserId = messageCreateDto.UserId,
                Metadata = messageCreateDto.Metadata
            };

            await _messageRepository.CreateAsync(message);
            return message;
        }

        public async Task<AuthResult> DeleteAsync(string id)
        {
            var message = await _messageRepository.GetByIdAsync(id);
            if (message == null)
            {
                return new AuthResult { Success = false, Message = "Message not found." };
            }

            await _messageRepository.DeleteAsync(id);
            return new AuthResult { Success = true, Message = "Message deleted successfully." };
        }
    }

}
