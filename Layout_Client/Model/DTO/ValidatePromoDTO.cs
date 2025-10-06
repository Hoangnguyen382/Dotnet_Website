namespace Layout_Client.Model.DTO
{
    public class ValidatePromoDTO
    {
        public decimal Discount { get; set; }
        public int? PromoCodeId { get; set; }
        public string? Error { get; set; }
    }
}