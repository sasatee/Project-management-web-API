using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
             
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("employees")]
        public async Task<ActionResult<List<UserDetailDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();
            var employeeCount = employees.Count;
            return Ok(new { employees, employeeCount });
        }




    }
}
