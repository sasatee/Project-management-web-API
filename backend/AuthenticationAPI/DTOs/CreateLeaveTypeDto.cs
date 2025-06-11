using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class CreateLeaveTypeDto
    {
        [Required(ErrorMessage = "Leave name is required")]
        public string Name { get; set; }
        public int DefaultDays { get; set; }

    }

    public class ReponseLeaveType
    {
      
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int DefaultDays { get; set; }

    }

    public class UpdateLeaveTypeDto
    {
        [Required(ErrorMessage ="Leave name is required")]
        public string Name { get; set; }
        public int DefaultDays { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

}
