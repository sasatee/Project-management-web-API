namespace Payroll.Model
{
    public class Employee
    {
        public Guid  Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfJoining { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews { get; set; }
        public ICollection<Payrolls> Payrolls { get; set; }
    }
}
