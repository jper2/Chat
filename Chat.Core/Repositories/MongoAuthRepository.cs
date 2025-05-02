using Chat.Core.Models;
using MongoDB.Driver;

namespace Chat.Core.Repositories
{
    public class MongoAuthRepository : IAuthRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoAuthRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("users");
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }
    }

}
