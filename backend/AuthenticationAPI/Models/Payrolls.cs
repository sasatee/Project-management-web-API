using System.ComponentModel.DataAnnotations;

namespace Payroll.Model
{
    public class Payrolls
    {
       
        public Guid Id { get; set; }
        public DateTime PayDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
