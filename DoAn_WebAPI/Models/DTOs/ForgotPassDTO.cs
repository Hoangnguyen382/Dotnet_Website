
using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models.DTOs
{
    public class ForgotPassDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}