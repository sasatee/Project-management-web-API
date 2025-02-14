using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Controllers
{
    public class CreateLeaveAllocationForYearDto
    {
        [Required]
        public Guid LeaveTypeId { get; set; }
        
        [Required]
        [Range(1900, 9999)]
        public int Period { get; set; }

        public Guid AppUserId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveAllocationController(ILeaveAllocationService leaveAllocationService, IEmployeeRepository employeeRepository)
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
                    dto.Period,dto.AppUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
