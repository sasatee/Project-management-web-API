using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase
    {

        private readonly ILeaveRequestRepository _repository;

        public LeaveRequestsController(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public string GetAll()
        {
            return "S";
        }

        [HttpPost]

        public async Task<ActionResult<LeaveRequestDto>> CreateLeaveRequest(CreateLeaveRequestDto createDto)
        {
            var leaveRequest = new LeaveRequest
            {
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                RequestComments = createDto.RequestComments,
                LeaveTypeId = createDto.LeaveTypeId,
                DateRequested = DateTime.UtcNow,
                //RequestingEmployeeId = _userService.GetUserId()
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

            return CreatedAtAction(nameof(CreateLeaveRequest), new { id = dto.Id }, dto);

        }
    }
}
