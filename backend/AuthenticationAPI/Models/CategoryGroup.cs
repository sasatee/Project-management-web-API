using Payroll.Model;
using System.Collections.Generic;

namespace AuthenticationAPI.Models
{
    public class CategoryGroup
    {
        public CategoryGroup()
        {
            SalaryProgressions = new List<SalaryProgression>();
            Employees = new List<Employee>();
            SalarySteps = new List<SalaryStep>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }    

        // Navigation properties
        public ICollection<SalaryProgression> SalaryProgressions { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<SalaryStep> SalarySteps { get; set; }
    }
}
