using System.ComponentModel.DataAnnotations;
namespace DoAn_WebAPI.Models.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }   
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }  
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Role { get; set; } = "User";
    }
}