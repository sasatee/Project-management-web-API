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




        [HttpGet("{employeeId}")]
        public async Task<IActionResult> CalculateSalaryPayroll( [FromQuery] Guid categoryId, [FromRoute] Guid employeeId, [FromQuery] int? YearOfService)
        {
            var salary = await _payrollService.CalculateDynamicSalary(categoryId, employeeId, YearOfService);
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
