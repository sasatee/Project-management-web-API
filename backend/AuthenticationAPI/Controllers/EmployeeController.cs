using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;
using System.Security.Claims;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepository<Employee> _EmpRepo;
        public EmployeeController(IEmployeeRepository employeeRepository,IRepository<Employee> employmentRepo)
        {
            _employeeRepository = employeeRepository;
            _EmpRepo = employmentRepo;

        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet("employees")]
        public async Task<ActionResult<List<UserDetailDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();
            var employeeCount = employees.Count;
            return Ok(new { employees, employeeCount });
        }

       // [Authorize(Roles = "ADMIN")]
        [HttpPost("create-a-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeRepository.CreateEmployee(Guid.NewGuid().ToString(), employeeDto);
            return Ok(result);
        }


      
    }
}
