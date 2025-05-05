using Chat.Core.Hubs;
using Chat.Core.Models;
using Chat.Core.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, IHubContext<ChatHub> hubContext, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            _logger.LogInformation("GetAllAsync called to retrieve all messages.");

            try
            {
                var messages = await _messageRepository.GetAllAsync();
                _logger.LogInformation("Successfully retrieved {Count} messages.", messages.Count());
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all messages.");
                throw;
            }
        }

        public async Task<Message> GetByIdAsync(string id)
        {
            _logger.LogInformation("GetByIdAsync called for message ID: {MessageId}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid message ID provided to GetByIdAsync.");
                throw new ArgumentException("Message ID must not be empty.");
            }

            try
            {
                var message = await _messageRepository.GetByIdAsync(id);
                if (message == null)
                {
                    _logger.LogWarning("Message not found with ID: {MessageId}", id);
                }
                else
                {
                    _logger.LogInformation("Message retrieved successfully with ID: {MessageId}", id);
                }
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the message with ID: {MessageId}", id);
                throw;
            }
        }

        public async Task<Message> CreateAsync(MessageCreateDto messageCreateDto)
        {
            _logger.LogInformation("CreateAsync called to create a new message.");

            if (messageCreateDto == null || string.IsNullOrWhiteSpace(messageCreateDto.Content))
            {
                _logger.LogWarning("Invalid input data for creating a message: {@MessageCreateDto}", messageCreateDto);
                throw new ArgumentException("Message content must not be empty.");
            }

            var message = new Message
            {
                Content = messageCreateDto.Content,
                Type = messageCreateDto.Type,
                CreatedAt = DateTime.UtcNow,
                UserId = messageCreateDto.UserId,
                Metadata = messageCreateDto.Metadata
            };

            try
            {
                await _messageRepository.CreateAsync(message);
                _logger.LogInformation("Message created successfully with ID: {MessageId}", message.Id);

                if (!string.IsNullOrEmpty(message.Id))
                {
                    await _hubContext.Clients.All.SendAsync("MessageAdded", message);
                    _logger.LogInformation("MessageAdded event broadcasted for message ID: {MessageId}", message.Id);
                }

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new message.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation("DeleteAsync called for message ID: {MessageId}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid message ID provided to DeleteAsync.");
                throw new ArgumentException("Message ID must not be empty.");
            }

            try
            {
                var message = await _messageRepository.GetByIdAsync(id);
                if (message == null)
                {
                    _logger.LogWarning("Message not found with ID: {MessageId}", id);
                    return false;
                }

                await _messageRepository.DeleteAsync(id);
                _logger.LogInformation("Message deleted successfully with ID: {MessageId}", id);

                await _hubContext.Clients.All.SendAsync("MessageDeleted", id);
                _logger.LogInformation("MessageDeleted event broadcasted for message ID: {MessageId}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message with ID: {MessageId}", id);
                throw;
            }
        }
    }
}

