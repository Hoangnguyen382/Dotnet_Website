using System.ComponentModel.DataAnnotations;
namespace DoAn_WebAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        public int UserID { get; set; }

        public int RestaurantID { get; set; }

        public int? MenuItemID { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual MenuItem? MenuItem { get; set; }
    }
}