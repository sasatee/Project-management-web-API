using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class EmployeeDto
    {


        [Required]
        public Guid JobTitleId { get;  set; }
        [Required]
        public Guid DepartmentId { get;  set; }
        public string Address { get;  set; }
        public string Phone { get;  set; }

        [EmailAddress]
        public string Email { get;  set; }
        public string LastName { get;  set; }
        public string FirstName { get;  set; }
    }
}
