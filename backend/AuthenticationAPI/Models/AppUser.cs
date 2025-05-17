using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models
{
    public class AppUser : IdentityUser
    {
        public enum UserGender
        {
            Male,
            Female,
            Other
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateJoined { get; set; }
        public string? GoogleId { get; set; }
        public string? GooglePicture { get; set; }
        public UserGender Gender { get; set; }

        public virtual ICollection<LeaveAllocation> LeaveAllocations { get; set; } = new List<LeaveAllocation> { };
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest> { };
    }
}
