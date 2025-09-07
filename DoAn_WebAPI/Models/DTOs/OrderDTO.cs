using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models.DTOs
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
        [Required(ErrorMessage = "DeliveryAddress is required")]
        [MaxLength(255, ErrorMessage = "DeliveryAddress cannot exceed 255 characters")]
        public string? DeliveryAddress { get; set; }
        [Required(ErrorMessage = "PaymentStatus is required")]
        [MaxLength(50, ErrorMessage = "PaymentStatus cannot exceed 50 characters")]
        public string? PaymentStatus { get; set; }  // Paid, Pending, Failed
        [MaxLength(255, ErrorMessage = "Note cannot exceed 255 characters")]
        public string? Note { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }  // Pending, Completed, Cancelled

        [Required(ErrorMessage = "PhoneNumber is required")]
        [MaxLength(10, ErrorMessage = "PhoneNumber cannot exceed 10 characters")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "PaymentMethod is required")]
        [MaxLength(50, ErrorMessage = "PaymentMethod cannot exceed 50 characters")]
        public string? PaymentMethod { get; set; }  // Cash, Card, Online, etc.
        public decimal TotalAmount { get; set; }  
    }

    
}
