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

        [HttpGet("calculate-salary")]
        public async Task<IActionResult> CalculateSalary([FromQuery] int yearsOfService, [FromQuery]  Guid id)
        {
            var salary = await _payrollService.CalculateSalary(yearsOfService, id);
            return Ok(new {result=salary});
        }
    }

}
