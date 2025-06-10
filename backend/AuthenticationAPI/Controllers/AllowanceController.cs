using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AllowanceController : Controller
    {

        private readonly IRepository<Allowance> _repository;

        public AllowanceController(IRepository<Allowance> repository)
        {
            _repository = repository;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAllowances()
        {

           var allowances = await _repository.GetAll();
            return Ok(allowances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAllowanceDto>> GetAllowancebyId([FromRoute] Guid id)
        {

            Allowance result = await _repository.FindByIdAsync(id);

            return Ok(result);

        }


        [HttpGet("GetAllowanceWith/{employeeId}")]
        public async Task<ActionResult<List<Allowance>>> GetAllowanceById([FromRoute] Guid employeeId)
        {

            var allowances = await _repository.GetAll(s => s.EmployeeId == employeeId);


            return Ok(allowances);



        }

      

        [HttpPost("create/many/allowances")]
        public async Task<IActionResult> AssignMultipleAllowances([FromBody] BulkAllowanceAssignDto dto)
        {
            List<Allowance> allowances = dto.Allowances.Select(a => new Allowance
            {
                Id = Guid.NewGuid(),
                TypeName = a.TypeName,
                Description = a.Description,
                EffectiveDate = a.EffectiveDate,
                Remarks = a.Remarks,
                ModifiedAt = DateTime.UtcNow,
                EmployeeId = dto.EmployeeId
            }).ToList();

            foreach (var allowance in allowances)
            {
                await _repository.AddAsync(allowance);
            }
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllowanceById), new { employeeId = dto.EmployeeId }, new { result = allowances, isSuccess = true });
        }



        [HttpPost]
        public async Task<ActionResult<AllowanceCreateDto>> CreateAllowance([FromBody] AllowanceCreateDto dto)
        {

            Allowance obj = new Allowance()
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                EffectiveDate = DateTime.UtcNow,
                EmployeeId= dto.EmployeeId,
                Remarks= dto.Remarks,
                ModifiedAt = DateTime.UtcNow,
                TypeName = dto.TypeName,
            };
            await _repository.AddAsync(obj);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllowanceById),new {id = obj.Id}, new { result = obj, isSuccess = true });

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AllowanceCreateDto>> UpdateAllowance([FromBody] AllowanceCreateDto dto,[FromRoute] Guid id )
        {

          Allowance existing = await _repository.FindByIdAsync(id);

          
             
            if (existing == null)
            {
               

                    return NotFound(new { isSucess = false, Message = $"attendence not found with {id}" });

                
            }

            existing.Remarks = dto.Remarks;
            existing.EffectiveDate = DateTime.UtcNow;
            //existing.EmployeeId = dto.EmployeeId;
            existing.TypeName = dto.TypeName;
            existing.Description = dto.Description;
            existing.ModifiedAt = DateTime.UtcNow;
            existing.Remarks = dto.Remarks;


                
         
            _repository.Update(existing);
            await _repository.SaveChangesAsync();

            return NoContent();

        }
    }
}
