using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class UpdateAttencesDtos
    {
     
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan CheckInTime { get; set; }
        [Required]
        public TimeSpan CheckOutTime { get; set; }
        [Required]
        public decimal OvertimeHours { get; set; }
 


    }
}
