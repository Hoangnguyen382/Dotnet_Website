namespace DoAn_WebAPI.Models
{
    public class MessageImage
    {
        public int MessageImageId { get; set; }
        public int MessageId { get; set; }
        public string ImageUrl { get; set; }
        public Message Message { get; set; }
    }
}

