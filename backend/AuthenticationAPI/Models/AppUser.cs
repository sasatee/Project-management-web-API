using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models
{
    public class AppUser:IdentityUser
    {
        public string? Fullname { get; set; }
    }
}
