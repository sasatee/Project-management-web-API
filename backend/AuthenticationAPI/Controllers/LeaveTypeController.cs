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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveTypeDto>>> GetLeaveTypes()
        {
          var LeaveTypes = await _repository.GetAllAsync();

            if(LeaveTypes == null) return NotFound();
            var dto = LeaveTypes.Select(lt => new LeaveTypeDto
            {
                Name = lt.Name,
                DefaultDays = lt.DefaultDays,
                Id = lt.Id,
            }
            );
            return Ok(dto);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDto>> GetLeaveType(Guid id)
        {
            var leaveType = await _repository.GetByIdAsync(id);
            if (leaveType == null) return NotFound();

            var dto = new LeaveTypeDto()
            {
                Id = leaveType.Id,
                Name = leaveType.Name,
                DefaultDays = leaveType.DefaultDays,
            };
            return Ok(dto);
        }




        [HttpPost]
        public async Task<ActionResult<LeaveTypeDto>> CreateLeaveType([FromBody] LeaveTypeDto dto)
        {
            // Map the DTO to the LeaveType entity
            var leaveTypeEntity = new LeaveType()
            {
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                DefaultDays = dto.DefaultDays,
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

      



     

      
    }
}
