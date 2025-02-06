namespace Payroll.Model
{

    public class PerformanceReview
    {
        public Guid Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Comments { get; set; }
        public int Score { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
