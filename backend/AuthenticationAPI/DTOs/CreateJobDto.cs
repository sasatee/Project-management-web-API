using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class CreateJobDto
    {

        [Required(ErrorMessage = "Job Title is required")]
        public string Title { get; set; }
        public decimal BaseSalary { get; set; }
        public int Grade { get; set; }
    }
}