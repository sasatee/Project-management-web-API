using System.ComponentModel.DataAnnotations;

namespace Payroll.Model
{
    public class EmployeeTraining
    {
        [Key] 
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid TrainingId { get; set; }
        public Training Training { get; set; }
    }
}
