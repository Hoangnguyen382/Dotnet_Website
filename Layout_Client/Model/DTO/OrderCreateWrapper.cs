using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Layout_Client.Model.DTO;

namespace Layout_Client.Models.DTOs
{
    public class OrderCreateWrapper
    {
        [Required]
        public OrderRequestDTO Order { get; set; } = new();

        [Required]
        public List<OrderDetailRequestDTO> OrderDetails { get; set; } = new();
    }
}
