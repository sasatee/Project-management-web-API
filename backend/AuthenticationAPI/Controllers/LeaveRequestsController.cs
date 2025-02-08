using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase

    {

        private readonly ILeaveRequestRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        public LeaveRequestsController(ILeaveRequestRepository repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;

        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequests()
        {
            var leaveRequests = await _repository.GetAllAsync();
            var dtos = leaveRequests.Select(lr => new LeaveRequestDto
            {
                Id = lr.Id,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                RequestComments = lr.RequestComments,
                Approved = lr.Approved,
                Cancelled = lr.Cancelled,
                LeaveTypeId = lr.LeaveTypeId,
                LeaveTypeName = lr.LeaveType.Name
            });
            return Ok(dtos);
        }

        [HttpGet("GetMyLeaves")]
   
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetMyLeaveRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }

            var leaveRequests = await _repository.GetLeaveRequestsByEmployee(currentUser.Id);
            var dtos = leaveRequests.Select(lr => new LeaveRequestDto
            {
                Id = lr.Id,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                RequestComments = lr.RequestComments,
                Approved = lr.Approved,
                Cancelled = lr.Cancelled,
                LeaveTypeId = lr.LeaveTypeId,
                LeaveTypeName = lr.LeaveType.Name
            });
            return Ok(dtos);
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<LeaveRequestDto>> CreateLeaveRequest(CreateLeaveRequestDto createDto)
        {
            if (!User.IsInRole("ADMIN"))
            {
                return Forbid();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }
            if (!Guid.TryParse(currentUser.Id, out Guid userId))
            {
                return BadRequest("Invalid user ID");
            }

            var leaveRequest = new LeaveRequest
            {
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                RequestComments = createDto.RequestComments,
                LeaveTypeId = createDto.LeaveTypeId,
                DateRequested = DateTime.UtcNow,
                RequestingEmployeeId = userId,

                Approved = false,
            };

            await _repository.CreateAsync(leaveRequest);

            var dto = new LeaveRequestDto
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                RequestComments = leaveRequest.RequestComments,
                LeaveTypeId = leaveRequest.LeaveTypeId
            };

            return CreatedAtAction(nameof(GetLeaveRequests), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Administrator")]
        public  Task<IActionResult> ApproveLeaveRequest(int id, [FromBody] bool approved)
        {
            throw new NotImplementedException();
        }
    }
}
