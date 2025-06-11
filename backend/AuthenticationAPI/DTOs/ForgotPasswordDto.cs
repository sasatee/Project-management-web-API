using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class ForgotPasswordDto
    {
        [EmailAddress(ErrorMessage = "Email address is required")]
        public  string  Email  { get; set; }
    }
}
