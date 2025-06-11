using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
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
                Id = deduction.Id,
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
