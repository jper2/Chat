using Chat.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Repositories
{
    public class MongoDBMessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MongoDBMessageRepository(IMongoClient client, IOptions<MongoDbSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _messages = database.GetCollection<Message>("Messages");
        }

        public async Task<Message> GetByIdAsync(string id) =>
            await _messages.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Message>> GetAllAsync() =>
            await _messages.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Message message) =>
            await _messages.InsertOneAsync(message);

        public async Task DeleteAsync(string id) =>
            await _messages.DeleteOneAsync(m => m.Id == id);
    }
}
