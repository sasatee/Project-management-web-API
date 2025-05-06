using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {
        private readonly ICreateLeaveAllocationService _leaveAllocationService;
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveAllocationController(ICreateLeaveAllocationService leaveAllocationService, IEmployeeRepository employeeRepository)
        {
            _leaveAllocationService = leaveAllocationService;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("create-for-year")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateLeaveAllocationsForYear([FromBody] CreateLeaveAllocationForYearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(dto.AppUserId.ToString(), out _))
            {
                return BadRequest(new { message = "Invalid AppUserId format" });
            }

            try
            {
                var result = await _leaveAllocationService.CreateLeaveAllocationsForYear(
                    dto.LeaveTypeId, // Convert Guid to int
                    dto.Period,dto.AppUserId,dto.EmployeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
