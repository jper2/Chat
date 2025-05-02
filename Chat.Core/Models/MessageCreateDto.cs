namespace Chat.Core.Models
{
    public class MessageCreateDto
    {
        public string Content { get; set; }
        public string Type { get; set; } // Text, Image, Chart, Table
        public string UserId { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

}
