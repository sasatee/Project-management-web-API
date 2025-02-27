using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; 

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

        [Authorize(Roles = "ADMIN")]
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


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { isSuccess = false, message = "User ID not found" });
            }


            var result = await _employeeRepository.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

            // Convert IResult to IActionResult
            if (result is ObjectResult objectResult)
            {
                return objectResult;
            }
            return StatusCode(500, new { isSuccess = false, message = "An unexpected error occurred" });
        }
    }
}
