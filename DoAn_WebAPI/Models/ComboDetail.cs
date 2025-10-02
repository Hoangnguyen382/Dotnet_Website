using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models
{
    public class ComboDetail
    {
        [Key]
        public int ComboDetailID { get; set; }

        [Required]
        public int ComboID { get; set; }

        [Required]
        public int MenuItemID { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        // Navigation
        public virtual Combo Combo { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
    