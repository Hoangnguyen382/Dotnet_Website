using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models.DTOs
{
    public class ReviewResponseDTO
    {
        public int ReviewID { get; set; }
        public int? RestaurantID { get; set; }
        public int? MenuItemID { get; set; }
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }

    public class ReviewRequestDTO
    {
        public int? RestaurantID { get; set; }
        public int? MenuItemID { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

        