using Chat.Core.Models;
using MongoDB.Driver;

namespace Chat.Core.Repositories
{
    public class MongoDBUsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoDBUsersRepository(IMongoDbSettings settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);
                _users = database.GetCollection<User>("users");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to connect to the database.", ex);
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                return await _users.Find(u => u.Username.Trim().ToLower() == username.Trim().ToLower()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the user.", ex);
            }
        }
    }
}