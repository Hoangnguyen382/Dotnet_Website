namespace Layout_Client.Model.DTO
{
    public class PromoValidateResponse
    {
        public bool IsValid { get; set; }
        public decimal Discount { get; set; }
        public int PromoCodeId { get; set; }
        public string? Message { get; set; }
    }
}
