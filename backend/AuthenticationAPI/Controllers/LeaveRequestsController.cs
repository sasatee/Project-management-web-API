using AuthenticationAPI.IRepository.IRepository;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase

    {

        private readonly ILeaveRequestRepository _leaveRequestrepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly UserManager<AppUser> _userManager;
     
        public LeaveRequestsController(ILeaveRequestRepository repository, UserManager<AppUser> userManager,ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveRequestrepository = repository;
            _userManager = userManager;
            _leaveTypeRepository = leaveTypeRepository;
            
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN,EMPLOYEE")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequests()
        {
            var leaveRequests = await _leaveRequestrepository.GetAllAsync();
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetMyLeaveRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }
            
     
            
            

            var leaveRequests = await _leaveRequestrepository.GetLeaveRequestsByEmployee(currentUser.Id.ToUpper());

            var dtos = leaveRequests.Select(lr => new LeaveRequestDto
            {
                Id = lr.Id,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                RequestComments = lr.RequestComments,
                Approved = lr.Approved,
                Cancelled = lr.Cancelled,
                LeaveTypeId = lr.LeaveTypeId,
                LeaveTypeName = lr.LeaveType?.Name
            }).ToList();

            return Ok(new
            {
                UserId = currentUser.Id,
                TotalLeave = dtos.Count,
                UserLeave = dtos
            });
        }




        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LeaveRequestDto>> CreateLeaveRequest(CreateLeaveRequestDto createDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }

            if (!Guid.TryParse(currentUser.Id, out Guid parsedGuid))
            {
                return BadRequest("Invalid user ID format");
            }

            var leaveRequest = new LeaveRequest
            {
                StartDate = DateTime.UtcNow,
                EndDate = createDto.EndDate,
                RequestComments = createDto.RequestComments,
                LeaveTypeId = createDto.LeaveTypeId,
                DateRequested = DateTime.UtcNow,
                AppUserId = currentUser.Id,
                Approved = false,
                Cancelled = false,
                RequestingEmployeeId = parsedGuid,
                Employees = new List<Employee>()
            };

            await _leaveRequestrepository.CreateAsync(leaveRequest);

            var dto = new LeaveRequestDto
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                RequestComments = leaveRequest.RequestComments,
                LeaveTypeId = leaveRequest.LeaveTypeId,
                LeaveTypeName = leaveRequest.LeaveType?.Name,
                Approved = leaveRequest.Approved,
                Cancelled = leaveRequest.Cancelled
            };

            return CreatedAtAction(nameof(GetLeaveRequests), new { id = dto.Id }, dto);
        }



        [HttpPut("{id}/approve")]
      //  [Authorize(Roles = "ADMIN")]
        public  async Task<IActionResult> ApproveLeaveRequest(Guid id, [FromBody] bool approved)
        {
           if(!await _leaveRequestrepository.Exists(id)) return NotFound();

            await _leaveRequestrepository.ChangeApprovalStatus(id, approved);
            return Ok(new {isSuccess = true, Message = $"Leave has been approved with approve id {id}"});
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLeaveRequest(Guid id)
        {
            await _leaveRequestrepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
