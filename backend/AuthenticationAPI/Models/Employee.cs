using AuthenticationAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Model
{
    public class Employee
    {
        public Employee()
        {
            Attendances = new List<Attendance>();
            PerformanceReviews = new List<PerformanceReview>();
            Payrolls = new List<Payrolls>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }

        [ForeignKey(nameof(Department))]
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey(nameof(JobTitle))]
        public Guid? JobTitleId { get; set; }
        public JobTitle? JobTitle { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews { get; set; }
        public ICollection<Payrolls> Payrolls { get; set; }
        public ICollection<Allowance> Allowances { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public string? NIC {  get; set; }

        public int YearsOfService { get; set; }
        public decimal CurrentSalary { get; set; }

        [ForeignKey(nameof(CategoryGroup))]
        public Guid? CategoryGroupId { get; set; }
        public CategoryGroup? CategoryGroup { get; set; }




        
    }
}
