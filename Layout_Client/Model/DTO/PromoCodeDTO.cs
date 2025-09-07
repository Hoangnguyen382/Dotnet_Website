using System.ComponentModel.DataAnnotations;

namespace Layout_Client.Models.DTOs
{
    public class PromoCodeResponseDTO
    {
        public int PromoCodeID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public PromoCodeType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int RestaurantID { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int? MinQuantity { get; set; }
        public int? GiftMenuItemID { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercent { get; set; }
    }
        public enum PromoCodeType
    {
        AmountDiscount,    // Giảm tiền theo tổng đơn hàng
        QuantityDiscount,  // Giảm tiền theo số lượng món
    }
}