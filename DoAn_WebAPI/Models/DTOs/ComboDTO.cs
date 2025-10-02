namespace DoAn_WebAPI.Models.DTOs
{
    public class ComboRequestDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
       
    }
    public class ComboResponseDTO
    {
        public int ComboID { get; set; }
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}