namespace Payroll.Model
{
    public class Training
    {
        public Guid Id { get; set; }
        public string TrainingName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    }
}
