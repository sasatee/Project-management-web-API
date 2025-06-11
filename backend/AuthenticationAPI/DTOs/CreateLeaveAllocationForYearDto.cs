using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Controllers
{
    public class CreateLeaveAllocationForYearDto
    {
        [Required]
        public Guid LeaveTypeId { get; set; }

        [Required]
        [Range(1900, 9999)]
        public int Period { get; set; }

        [Required]
        public Guid AppUserId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
    }
}
