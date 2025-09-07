using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DoAn_WebAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int MenuItemID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public virtual Order? Order { get; set; }
        public virtual MenuItem? MenuItem { get; set; }
    }

}