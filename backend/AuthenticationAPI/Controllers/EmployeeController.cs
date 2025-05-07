using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepository<Employee> _emploRepo;
        private readonly IRepository<JobTitle> _jobTitleRepo;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IRepository<Employee> employmentRepo,
            IRepository<JobTitle> jobTitleRepo)
        {
            _employeeRepository = employeeRepository;
            _emploRepo = employmentRepo;
            _jobTitleRepo = jobTitleRepo;
        }

        //[Authorize(Roles = "ADMIN")]
        //get app user with employee id
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

        //Get all employee
        [HttpGet("all-employees")]
        public async Task<IActionResult> GetAllEmployeesList()
        {

            return Ok (await _emploRepo.GetAll());



        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            return Ok(await _emploRepo.FindByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, UpdateEmployeeDto updateEmployeeDto, [FromQuery] Guid? jobTitleId = null)
        {
            var employeeModel = await _emploRepo.FindByIdAsync(id);
            if (employeeModel == null)
            {
                return NotFound(new { isSuccess = false, message = "Employee not found" });
            }

            // Update basic employee information
            employeeModel.FirstName = updateEmployeeDto.FirstName;
            employeeModel.LastName = updateEmployeeDto.LastName;
            employeeModel.Phone = updateEmployeeDto.Phone;
            employeeModel.Address = updateEmployeeDto.Address;

            // Update JobTitle only if jobTitleId is provided in query string
            if (jobTitleId.HasValue)
            {
                var jobTitle = await _jobTitleRepo.FindByIdAsync(jobTitleId.Value);
                if (jobTitle == null)
                {
                    return BadRequest(new { isSuccess = false, message = "Invalid JobTitle ID" });
                }

                // Update the job title if title is provided
                if (!string.IsNullOrEmpty(updateEmployeeDto.JobTitle))
                {

                    // Update basic job title information
                    jobTitle.Title = updateEmployeeDto.JobTitle;

                   // jobTitle.BaseSalary =

                    _jobTitleRepo.Update(jobTitle);
                }

                employeeModel.JobTitleId = jobTitleId;
                employeeModel.JobTitle = jobTitle;
            }

            _emploRepo.Update(employeeModel);
            await _emploRepo.SaveChangesAsync();

           
            var response = new
            {
                isSuccess = true,
                message = "Employee updated successfully",
                result = new
                {
                    employee = new
                    {
                        employeeModel.Id,
                        employeeModel.FirstName,
                        employeeModel.LastName,
                        employeeModel.Email,
                        employeeModel.Phone,
                        employeeModel.Address,
                        employeeModel.DateOfJoining,
                        employeeModel.DateOfLeaving,
                        employeeModel.DepartmentId,
                        employeeModel.JobTitleId,
                        JobTitle = employeeModel.JobTitle != null ? new
                        {   
                            employeeModel.JobTitle.Id,
                            employeeModel.JobTitle.Title,
                            employeeModel.JobTitle.BaseSalary,
                            employeeModel.JobTitle.Grade
                        } : null
                    }
                }
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            await _emploRepo.DeleteAsync(id);
            await _emploRepo.SaveChangesAsync();
            return NoContent();
        }




    }
}
