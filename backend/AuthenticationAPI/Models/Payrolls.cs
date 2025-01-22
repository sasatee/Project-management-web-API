using System.ComponentModel.DataAnnotations;

namespace Payroll.Model
{
    public class Payrolls
    {
        [Key]
        public int PayrollId { get; set; }
        public DateTime PayDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
