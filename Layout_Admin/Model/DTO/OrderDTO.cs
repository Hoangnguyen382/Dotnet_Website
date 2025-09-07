using System.ComponentModel.DataAnnotations;

namespace Layout_Admin.Model.DTO
{
    public class OrderResponseDTO
    {
        public int OrderID { get; set; }
        public int PromoCodeID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }
    }
    //Order Request DTOs
    public class OrderRequestDTO
    {
        public string? Code { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentStatus { get; set; }  // Paid, Pending, Failed
        public string? Note { get; set; }
        public string? Status { get; set; }  // Pending, Completed, Cancelled
        public string? PhoneNumber { get; set; }
        public string? PaymentMethod { get; set; }  // Cash, Card, Online, etc.
           
    }

    
}
