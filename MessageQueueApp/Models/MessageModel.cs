namespace MessageQueueApp.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
