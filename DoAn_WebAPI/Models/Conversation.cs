namespace DoAn_WebAPI.Models
{
    public class Conversation
    {
        public int ConversationId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Message> Messages { get; set; }
        public virtual Order Order { get; set; }
    }
}

