using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class AllowanceCreateDto
    {
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Remarks { get; set; }

        [Required(ErrorMessage = "employee id is required")]
        public Guid EmployeeId { get; set; }
        public decimal Amount { get; set; }
    }
}
