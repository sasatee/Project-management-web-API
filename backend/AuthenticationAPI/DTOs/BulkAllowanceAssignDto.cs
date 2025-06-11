using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class BulkAllowanceAssignDto
    {
        [Required(ErrorMessage = "employee id is required")]
        public Guid EmployeeId { get; set; }
        public List<AllowanceCreateDto> Allowances { get; set; }
    }
}
