using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class GetUserEmployeeDto 
    {
        [Required]
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string> Roles { get; set; }
        public string? AppUserId { get; set; }

        [Required]
        public Guid? EmployeeId { get; set; }
    }
}