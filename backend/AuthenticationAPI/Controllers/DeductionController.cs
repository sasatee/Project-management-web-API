using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionController : ControllerBase
    {
        private readonly IRepository<Deduction> _deductionRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public DeductionController(IRepository<Deduction> deductionRepository , IRepository<Employee> employeeRepository)
        {
            _deductionRepository = deductionRepository;
            _employeeRepository = employeeRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetDeductions()
        {
            return Ok(await _deductionRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeduction([FromRoute] Guid id)
        {
            return Ok(await _deductionRepository.GetAll(p => p.Id == id));


        }

        [HttpGet("GetDeductionWith/{employeeId}")]
        public async Task<ActionResult<List<Allowance>>> GetAllowanceById([FromRoute] Guid employeeId)
        {

            var allowances = await _deductionRepository.GetAll(s => s.EmployeeId == employeeId);


            return Ok(allowances);



        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteDeduction([FromRoute] Guid id)
        {
           Deduction? exist =  await _deductionRepository.FindByIdAsync(id);
            if (exist  is null)
            {
                return NotFound();
            }
            await _deductionRepository.DeleteAsync(id);
            await _deductionRepository.SaveChangesAsync();
            return NoContent();


        }


        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateDeduction([FromRoute] Guid id, CreateDeductionRequestDto dto)
        {
            Deduction? exist = await _deductionRepository.FindByIdAsync(id);
            if (exist is null)
            {
                return NotFound();
            }

            exist.EffectiveDate = DateTime.Now;
            exist.ModifiedDate = DateTime.Now;
            exist.TypeName = dto.TypeName;
            exist.Remarks = dto.Remarks;
            exist.Amount = dto.Amount;
       


             _deductionRepository.Update(exist);
            await _deductionRepository.SaveChangesAsync();
            return NoContent();


        }






        [HttpPost]
        public async Task<IActionResult> CreateDeduction([FromBody] CreateDeductionRequestDto dto)
        {
            Employee? employee = await _employeeRepository.FindByIdAsync(dto.EmployeeId);
            if (employee == null)
            {
                return BadRequest($"Employee with ID {dto.EmployeeId} does not exist.");
            }

            Deduction? deduction = new Deduction()
            {
                Amount = dto.Amount,
                EffectiveDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Remarks = dto.Remarks,
                EmployeeId = dto.EmployeeId,
                TypeName = dto.TypeName
            };

            await _deductionRepository.AddAsync(deduction);
            await _deductionRepository.SaveChangesAsync();

            var responseDto = new DeductionResponseDto
            {
             
                TypeName = deduction.TypeName,
                Amount = deduction.Amount,
                EffectiveDate = deduction.EffectiveDate,
                ModifiedDate = deduction.ModifiedDate,
                Remarks = deduction.Remarks,
                EmployeeId = deduction.EmployeeId,
                Employee = new EmployeeBasicDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                }
            };

            return CreatedAtAction(nameof(GetDeduction), new { id = deduction.Id }, new { result = responseDto, isSuccess = true });
        }
    }


 }
