using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_WebAPI.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemID { get; set; }

        public int RestaurantID { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ImageUrl { get; set; }

        public int CategoryID { get; set; }

        public virtual Restaurant? Restaurant { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ComboDetail>? ComboDetails { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}

