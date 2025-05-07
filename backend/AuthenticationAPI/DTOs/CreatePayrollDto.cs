using Payroll.Model;

namespace AuthenticationAPI.DTOs
{

    public class CreatePayrollDto
    {


        public DateTime PayDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }

        
    }
}
