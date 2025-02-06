namespace Payroll.Model
{

    public class Leave
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

}
