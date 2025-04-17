using AuthenticationAPI.Models;
using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _payrollService;

        public PayrollController(PayrollService payrollService)
        {
            _payrollService = payrollService;
        }

      
        
       
        [HttpGet("calculate-dynamic-salary")]
        public async Task<IActionResult> CalculateDynamicSalary([FromQuery] int yearsOfService, [FromQuery] Guid categoryId)
        {
            var salary = await _payrollService.CalculateDynamicSalary(yearsOfService, categoryId);
            return Ok(new { result = salary });
        }
        
      
    }
    
    public class SeedSalaryStepsRequest
    {
        public Guid CategoryGroupId { get; set; }
        public string CategoryName { get; set; }
        public List<SalaryStep> Steps { get; set; }
    }
}
