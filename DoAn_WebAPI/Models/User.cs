using System.ComponentModel.DataAnnotations;
namespace DoAn_WebAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int? RestaurantID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Role { get; set; }
        public string? TokenResetPassword { get; set; }
        public DateTime? ExpiresTokenReset { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}