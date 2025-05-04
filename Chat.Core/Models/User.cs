using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Models
{

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

}
