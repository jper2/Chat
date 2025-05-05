using Chat.Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace Chat.Core.Repositories
{
    public class MongoDBMessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;
        private readonly ILogger<MongoDBMessageRepository> _logger;

        public MongoDBMessageRepository(IMongoClient mongoClient, IMongoDbSettings settings, ILogger<MongoDBMessageRepository> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _messages = database.GetCollection<Message>("Messages");
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            _logger.LogInformation("GetAllAsync called to retrieve all messages from the database.");

            try
            {
                var messages = await _messages.Find(_ => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} messages from the database.", messages.Count);
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all messages from the database.");
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
                var message = await _messages.Find(m => m.Id == id).FirstOrDefaultAsync();
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

        public async Task CreateAsync(Message message)
        {
            _logger.LogInformation("CreateAsync called to insert a new message into the database.");

            if (message == null || string.IsNullOrWhiteSpace(message.Content))
            {
                _logger.LogWarning("Invalid message data provided to CreateAsync: {@Message}", message);
                throw new ArgumentException("Message content must not be empty.");
            }

            try
            {
                await _messages.InsertOneAsync(message);
                _logger.LogInformation("Message inserted successfully with ID: {MessageId}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting a new message into the database.");
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogInformation("DeleteAsync called for message ID: {MessageId}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid message ID provided to DeleteAsync.");
                throw new ArgumentException("Message ID must not be empty.");
            }

            try
            {
                var result = await _messages.DeleteOneAsync(m => m.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning("No message found to delete with ID: {MessageId}", id);
                }
                else
                {
                    _logger.LogInformation("Message deleted successfully with ID: {MessageId}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message with ID: {MessageId}", id);
                throw;
            }
        }
    }
}
