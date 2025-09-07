

namespace DoAn_WebAPI.Models.DTOs
{
    public class MenuItemReponseDTO
    {
        public int MenuItemID { get; set; }
        public int RestaurantID { get; set; }
        public int? CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class MenuItemRequestDTO
    {
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsAvailable { get; set; } = true;
       public string? ImageUrl { get; set; }
    }
}
