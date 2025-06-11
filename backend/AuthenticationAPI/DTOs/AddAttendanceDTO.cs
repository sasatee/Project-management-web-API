using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class AddAttendanceDTO {

        [Required(ErrorMessage = "employee id is required")]
        public Guid EmployeeId { get; set; }  
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
      
    }

}
