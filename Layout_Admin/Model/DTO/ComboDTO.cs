using System.ComponentModel.DataAnnotations;

namespace Layout_Admin.Model.DTO
{
    public class ComboRequestDTO
    {
        [Required(ErrorMessage = "Tên combo không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string? Description { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ảnh cho combo")]
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