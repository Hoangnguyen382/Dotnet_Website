using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_WebAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(255)]
        public string? OpeningHours { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(255)]
        public string? LogoUrl { get; set; }

        [Required]
        public int UserID { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<MenuItem>? MenuItems { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
