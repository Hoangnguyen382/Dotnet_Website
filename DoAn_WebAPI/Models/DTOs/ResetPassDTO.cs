using System.ComponentModel.DataAnnotations;

namespace DoAn_WebAPI.Models.DTOs
{
    public class ResetPassDTO
    {
        [Required (ErrorMessage = "Enter reset password")]
        public string? ResetToken { get; set; }

        [Required (ErrorMessage = "Enter new password")]
        public string? NewPassword { get; set; }
    }
}