using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly IRepository<LeaveAllocation> _repository;

        public LeaveAllocationController(ILeaveAllocationService leaveAllocationService,IRepository<LeaveAllocation> repository)
        {
            _leaveAllocationService = leaveAllocationService;
            _repository = repository;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveAllocation([FromRoute] Guid id)
        {
            var result = await _repository.GetAll(p => p.Id == id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveAllocations()
        {
            return Ok(await _repository.GetAll());
        }


        [HttpPost("create-for-year")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateLeaveAllocationsForYear([FromBody] CreateLeaveAllocationForYearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(dto.AppUserId.ToString(),  out var id))
            {
                return BadRequest(new { message = $"Invalid AppUserId format {id}" });
            }

            try
            {
                var result = await _leaveAllocationService.CreateLeaveAllocationsForYear(
                    dto.LeaveTypeId,
                    dto.Period,dto.AppUserId,dto.EmployeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
       

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLeaveAllocation(Guid id, [FromBody] RequestLeaveAllocationDtos leaveAllocationDtos)
        {

            LeaveAllocation? exist = await _repository.FindByIdAsync(id);

            if (exist is  null) return NotFound($"Leave allocation not found with {id}");


            exist.DateCreated = DateTime.UtcNow;
            exist.Period = leaveAllocationDtos.Period;




            _repository.Update(exist);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveAllocation(Guid id, [FromBody] RequestLeaveAllocationDtos leaveAllocationDtos)
        {

            LeaveAllocation? exist = await _repository.FindByIdAsync(id);

            if (exist is null) return NotFound($"Leave allocation not found with {id}");


            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

    }
}
