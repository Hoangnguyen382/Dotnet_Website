namespace DoAn_WebAPI.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<MessageImage> Images { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}
