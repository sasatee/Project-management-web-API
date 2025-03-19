using AuthenticationAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    public class SalaryProgression
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public decimal Salary { get; set; }
        public decimal Increment { get; set; }

        [ForeignKey(nameof(CategoryGroup))]
        public Guid CategoryGroupId { get; set; }
        public CategoryGroup CategoryGroup { get; set; }
    }
}
