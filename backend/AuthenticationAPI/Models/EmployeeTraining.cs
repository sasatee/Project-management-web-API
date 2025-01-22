using System.ComponentModel.DataAnnotations;

namespace Payroll.Model
{
    public class EmployeeTraining
    {
        [Key] 
        public int EmployeeTrainingId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int TrainingId { get; set; }
        public Training Training { get; set; }
    }
}
