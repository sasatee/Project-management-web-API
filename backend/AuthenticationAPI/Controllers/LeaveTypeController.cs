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
        private readonly IRepository<LeaveType> _leaveTypeRepository;  // only have update/delete operation 
        public LeaveTypeController(ILeaveTypeRepository repository,IRepository<LeaveType> leaveRepository )
        {
            _repository = repository;
            _leaveTypeRepository = leaveRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateLeaveTypeDto>>> GetLeaveTypes()
        {
          var LeaveTypes = await _repository.GetAllAsync();

            if(LeaveTypes == null) return NotFound();
            var dto = LeaveTypes.Select(lt => new ReponseLeaveType
            {
                Name = lt.Name,
                DefaultDays = lt.DefaultDays,
                Id = lt.Id,
            }
            );
            return Ok(dto);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<CreateLeaveTypeDto>> GetLeaveType(Guid id)
        {
            var leaveType = await _repository.GetByIdAsync(id);
            if (leaveType == null) return NotFound();

            var dto = new ReponseLeaveType()
            {
                Id = leaveType.Id,
                Name = leaveType.Name,
                DefaultDays = leaveType.DefaultDays,
            }; 
            return Ok(dto);
        }




        [HttpPost]
        public async Task<ActionResult<CreateLeaveTypeDto>> CreateLeaveType([FromBody] CreateLeaveTypeDto dto)
        {
          
            var leaveTypeEntity = new LeaveType()
            {
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                DefaultDays = dto.DefaultDays,
                Id = Guid.NewGuid(), 
                Name = dto.Name,   
            };

       
            await _repository.CreateAsync(leaveTypeEntity);

          
            var leaveTypeDto = new LeaveType()
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
            var leaveType = await _leaveTypeRepository.FindByIdAsync(id);
            if (leaveType == null)
            {
                return NotFound(new { isSuccess = "false", Message = "Job type leave is not found" });

            }

            leaveType.DefaultDays = updateLeaveDto.DefaultDays;
            leaveType.DateCreated = updateLeaveDto.DateCreated;
            leaveType.DateModified = DateTime.UtcNow;
            leaveType.Name = updateLeaveDto.Name;

            _leaveTypeRepository.Update(leaveType);
            await _leaveTypeRepository.SaveChangesAsync();
            return NoContent();


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType([FromRoute] Guid id)
        {

            var foundJobTypeId = _leaveTypeRepository.FindByIdAsync(id);
            if(foundJobTypeId == null)
            {
                return NotFound(new { isSuccess = false, Message = $"Leave type not found with {id}" });
            }
            await _leaveTypeRepository.DeleteAsync(id);
            await _leaveTypeRepository.SaveChangesAsync();
            return Ok(new { isSuccess = true, Message = "Successfully delete leave type" });


        }

    }



    }



