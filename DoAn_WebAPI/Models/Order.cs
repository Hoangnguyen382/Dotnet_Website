using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DoAn_WebAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public int? PromoCodeID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }  // Pending, Completed, Cancelled

        [MaxLength(255)]
        public string? DeliveryAddress { get; set; }
        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }  // Cash, Card, Online, etc.

        [MaxLength(50)]
        public string? PaymentStatus { get; set; } // Unpaid, Paid, Failed, etc.
        [MaxLength(255)]
        public string? Note { get; set; }
        public virtual User? User { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }

}