using System.ComponentModel.DataAnnotations;

namespace Layout_Admin.Models.DTO
{
    public class MenuItemResponseDTO
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
        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public int? CategoryID { get; set; }
        [Required(ErrorMessage = "Tên món không được để trống")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1000, int.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsAvailable { get; set; } = true;
       public string? ImageUrl { get; set; }
    }
}
