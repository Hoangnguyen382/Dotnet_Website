namespace DoAn_WebAPI.Models.DTOs
{
    public class ValidatePromoDTO
    {
        public decimal Discount { get; set; }
        public int? PromoCodeId { get; set; }
        public string? Error { get; set; }
    }
}