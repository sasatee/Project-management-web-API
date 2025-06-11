using Payroll.Model;

namespace AuthenticationAPI.Models
{
    public class Allowance
    {
        public Guid Id { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }
        public string? Remarks { get; set; }

        public decimal Amount { get; set; }
        public DateTime ModifiedAt { get; set; }
       
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
