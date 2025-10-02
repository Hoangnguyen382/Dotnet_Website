namespace Layout_Client.Model.DTO
{
    public class CreateMessageRequestDTO
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; } = ""; 
        public string Content { get; set; } = "";
        public List<string>? ImageUrls { get; set; }
    }
    public class MessageResponseDTO
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public List<MessageImageDTO> Images { get; set; }
    }
}