using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; } // Text, Image, Chart, Table
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        [BsonIgnoreIfNull]
        public Dictionary<string, object> Metadata { get; set; }
    }

}
