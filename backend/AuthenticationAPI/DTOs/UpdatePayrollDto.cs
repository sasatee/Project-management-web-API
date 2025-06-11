using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class UpdatePayrollDto
    {
            public DateTime PayDate { get; set; }
            [Required]
            public decimal BasicSalary { get; set; }
            public decimal Allowances { get; set; }
            public decimal Deductions { get; set; }
            public decimal NetPay { get; set; }

        }
}
