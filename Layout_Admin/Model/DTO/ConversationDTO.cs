namespace Layout_Admin.Model.DTO
{

    public class ConversationResponseDTO
    {
        public int ConversationId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateConversationRequestDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
    }
}
