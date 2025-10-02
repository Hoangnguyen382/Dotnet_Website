using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models.DTOs
{
    public class OrderDetailRequestDTO
    {
        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }
        public int? MenuItemID { get; set; }
        public int? ComboID { get; set; } 
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
    public class OrderDetailResponseDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int? ComboID { get; set; }
        public int? MenuItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    } 
}