using Payroll.Model;

namespace AuthenticationAPI.Models
{
    public class Allowances
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; }

        public  DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
       
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
