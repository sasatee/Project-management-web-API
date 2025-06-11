using Payroll.Model;

namespace AuthenticationAPI.Models
{
    public class Deduction
    {
        public Guid Id { get; set; }
        public string? TypeName { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Remarks { get; set; }
        //fk
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

}
