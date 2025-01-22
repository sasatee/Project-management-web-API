namespace Payroll.Model
{
    public class Training
    {
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    }
}
