using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Models.DTO
{
    public class OrderCreateWrapper
    {
        [Required]
        public OrderRequestDTO Order { get; set; } = new();

        [Required]
        public List<OrderDetailRequestDTO> OrderDetails { get; set; } = new();
    }
}
