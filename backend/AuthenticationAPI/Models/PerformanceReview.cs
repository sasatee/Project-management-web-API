namespace Payroll.Model
{

    public class PerformanceReview
    {
        public int PerformanceReviewId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Comments { get; set; }
        public int Score { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
