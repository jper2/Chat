using Chat.Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace Chat.Core.Repositories
{
    public class MongoDBUsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly ILogger<MongoDBUsersRepository> _logger;

        public MongoDBUsersRepository(IMongoClient mongoClient, IMongoDbSettings settings, ILogger<MongoDBUsersRepository> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
            _logger = logger;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            _logger.LogInformation("GetByUsernameAsync called for username: {Username}", username);

            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Invalid username provided to GetByUsernameAsync.");
                throw new ArgumentException("Username must not be empty.");
            }

            try
            {
                var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
                if (user == null)
                {
                    _logger.LogWarning("User not found with username: {Username}", username);
                }
                else
                {
                    _logger.LogInformation("User retrieved successfully with username: {Username}", username);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user with username: {Username}", username);
                throw;
            }
        }

        public async Task CreateUserAsync(User user)
        {
            _logger.LogInformation("CreateUserAsync called to insert a new user into the database.");

            if (user == null || string.IsNullOrWhiteSpace(user.Username))
            {
                _logger.LogWarning("Invalid user data provided to CreateUserAsync: {@User}", user);
                throw new ArgumentException("User data must not be empty.");
            }

            try
            {
                await _users.InsertOneAsync(user);
                _logger.LogInformation("User inserted successfully with username: {Username}", user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting a new user into the database.");
                throw;
            }
        }
    }
}

