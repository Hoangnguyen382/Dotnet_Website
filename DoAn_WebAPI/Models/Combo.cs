using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DoAn_WebAPI.Models
{
    public class Combo
    {
        [Key]
        public int ComboID { get; set; }

        [Required]
        public int RestaurantID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<ComboDetail> ComboDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
