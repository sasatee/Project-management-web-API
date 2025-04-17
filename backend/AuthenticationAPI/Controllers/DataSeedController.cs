using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly SeedSalaryForCategory _salaryStepSeeder;

        public DataSeedController(SeedSalaryForCategory salaryStepSeeder)
        {
            _salaryStepSeeder = salaryStepSeeder;
        }

        
        
        [HttpPost("seed-all")]
        public async Task<IActionResult> SeedAll()
        {
            await _salaryStepSeeder.SeedUTM1SalarySteps();
            await _salaryStepSeeder.SeedUTM2SalarySteps();
            await _salaryStepSeeder.SeedUTM3SalarySteps();
            return Ok(new { success = true, message = "All salary steps seeded successfully" });
        }
    }
} 