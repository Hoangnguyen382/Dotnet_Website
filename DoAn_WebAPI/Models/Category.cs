using System.ComponentModel.DataAnnotations;
namespace DoAn_WebAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? RestaurantID { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}