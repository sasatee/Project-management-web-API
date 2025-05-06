using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class CreateLeaveAllocationForYearDto
    {
        [Required]
        public Guid LeaveTypeId { get; set; }
        
        [Required]
        [Range(1900, 9999)]
        public int Period { get; set; }

        public Guid AppUserId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
