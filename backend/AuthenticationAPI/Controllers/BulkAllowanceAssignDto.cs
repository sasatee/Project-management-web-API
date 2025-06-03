using AuthenticationAPI.DTOs;

namespace AuthenticationAPI.Controllers
{
    public class BulkAllowanceAssignDto
    {
        public Guid EmployeeId { get; set; }
        public List<AllowanceCreateDto> Allowances { get; set; }
    }
}
