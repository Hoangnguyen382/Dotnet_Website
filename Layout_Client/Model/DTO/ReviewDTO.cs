using System.ComponentModel.DataAnnotations;

namespace Layout_Client.Model.DTO
{
    public class ReviewResponseDTO
    {
        public int ReviewID { get; set; }
        public int? RestaurantID { get; set; }
        public int? MenuItemID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class ReviewRequestDTO
    {
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn món ăn")]
        public int MenuItemID { get; set; }
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1–5")]
        public int Rating { get; set; }
         [Required(ErrorMessage = "Vui lòng nhập bình luận")]
        public string? Comment { get; set; }
    }
}

        