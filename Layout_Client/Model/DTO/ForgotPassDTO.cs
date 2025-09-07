using System.ComponentModel.DataAnnotations;

namespace Layout_Client.Model.DTO
{
    public class ForgotPassDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}