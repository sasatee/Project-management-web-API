using Payroll.Model;
using System.Collections.Generic;

namespace AuthenticationAPI.Models
{
    public class CategoryGroup
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; }    = string.Empty; 

       
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<SalaryStep>? SalarySteps { get; set; }
    }
}
