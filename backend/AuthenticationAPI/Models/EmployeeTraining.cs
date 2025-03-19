using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Model
{
    public class EmployeeTraining
    {
        [Key] 
        public Guid Id { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        public Training Training { get; set; }
    }
}
