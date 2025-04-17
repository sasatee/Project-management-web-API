using AuthenticationAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    public class SalaryStep
    {
        public Guid Id { get; set; }
        public decimal FromAmount { get; set; }
        public decimal Increment { get; set; }
        public decimal ToAmount { get; set; }
        
        [ForeignKey(nameof(CategoryGroup))]
        public Guid CategoryGroupId { get; set; }
        public CategoryGroup CategoryGroup { get; set; }
    }
} 