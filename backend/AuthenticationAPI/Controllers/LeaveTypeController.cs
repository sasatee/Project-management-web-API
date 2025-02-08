using AuthenticationAPI.DTOs;
using AuthenticationAPI.IRepository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeRepository _repository;
        public LeaveTypeController(ILeaveTypeRepository repository)
        {
            _repository = repository;
        }

       
        [HttpPost]
        public async Task<ActionResult<LeaveTypeDto>> CreateLeaveType([FromBody] LeaveTypeDto dto)
        {
            // Map the DTO to the LeaveType entity
            var leaveTypeEntity = new LeaveType()
            {
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                DefaultDays = 0,
                Id = Guid.NewGuid(), 
                Name = dto.Name,   
            };

            // Save the LeaveType to the repository
            await _repository.CreateAsync(leaveTypeEntity);

            // Map the created LeaveType entity to a DTO
            var leaveTypeDto = new LeaveTypeDto()
            {
                Id = leaveTypeEntity.Id,
                Name = leaveTypeEntity.Name,
                DateCreated = leaveTypeEntity.DateCreated,
                DateModified = leaveTypeEntity.DateModified,
                DefaultDays = leaveTypeEntity.DefaultDays,
            };

       
            return CreatedAtAction(nameof(CreateLeaveType), new { id = leaveTypeDto.Id }, leaveTypeDto);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Task<LeaveType> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public Task UpdateAsync(LeaveType leaveType)
        {
            throw new NotImplementedException();
        }
    }
}
