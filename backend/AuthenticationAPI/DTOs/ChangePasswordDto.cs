using System.ComponentModel.DataAnnotations;
namespace AuthenticationAPI.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string CurrentPassword { get; set; } = string.Empty;

      //  [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

}