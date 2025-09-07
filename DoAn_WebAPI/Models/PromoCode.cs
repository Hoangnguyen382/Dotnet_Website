using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DoAn_WebAPI.Models
{
    public class PromoCode
    {
        [Key]
        public int PromoCodeID { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        public string Description { get; set; }
        [Required]
        public PromoCodeType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        // Tham số điều kiện áp dụng cho từng loại promo code
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinOrderAmount { get; set; } // vd: > 2tr
        public int? MinQuantity { get; set; } // vd: > 10 món
        public int? GiftMenuItemID { get; set; } // Id món tặng kèm
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; } // Số tiền giảm
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercent { get; set; } // % giảm nếu có
        [Required]
        public int RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }

    }
    public enum PromoCodeType
    {
        AmountDiscount,    // Giảm tiền theo tổng đơn hàng
        QuantityDiscount,  // Giảm tiền theo số lượng món
    }
}