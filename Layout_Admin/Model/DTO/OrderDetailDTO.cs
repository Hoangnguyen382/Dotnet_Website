using System.ComponentModel.DataAnnotations;

namespace Layout_Admin.Model.DTO
{
    public class OrderDetailRequestDTO
    {
        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "MenuItemID is required")]
        public int MenuItemID { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "UnitPrice is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0")]
        public decimal UnitPrice { get; set; }
    }
    public class OrderDetailResponseDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public string? MenuItemName { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    } 
}