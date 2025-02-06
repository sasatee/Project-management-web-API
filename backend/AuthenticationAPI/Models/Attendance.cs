namespace Payroll.Model
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

}
