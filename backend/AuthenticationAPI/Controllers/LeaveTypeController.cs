using AuthenticationAPI.DTOs;
using AuthenticationAPI.IRepository.IRepository;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeRepository _repository;
        private readonly IRepository<LeaveType> _leaveRepository;  // only have update/delete operation 
        public LeaveTypeController(ILeaveTypeRepository repository,IRepository<LeaveType> leaveRepository )
        {
            _repository = repository;
            _leaveRepository = leaveRepository;
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



        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateLeaveTypeDto>> UpdateLeaveType([FromBody] UpdateLeaveTypeDto updateLeaveDto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var leaveType = await _leaveRepository.FindByIdAsync(id);
            if (leaveType == null)
            {
                return NotFound(new { isSuccess = "false", Message = "Job type leave is not found" });

            }

            leaveType.DefaultDays = updateLeaveDto.DefaultDays;
            leaveType.DateCreated = updateLeaveDto.DateCreated;
            leaveType.DateModified = updateLeaveDto.DateModified;
            leaveType.Name = updateLeaveDto.Name;

            _leaveRepository.Update(leaveType);
            await _leaveRepository.SaveChangesAsync();
            return NoContent();


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType([FromRoute] Guid id)
        {

            var foundJobTypeId = _leaveRepository.FindByIdAsync(id);
            if(foundJobTypeId == null)
            {
                return NotFound(new { isSuccess = false, Message = $"Leave type not found with {id}" });
            }
            await _leaveRepository.DeleteAsync(id);
            await _leaveRepository.SaveChangesAsync();
            return Ok(new { isSuccess = true, Message = "Successfully delete leave type" });


        }

    }



    }



