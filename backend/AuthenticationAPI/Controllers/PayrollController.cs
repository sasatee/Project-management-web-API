using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _payrollService;
        private readonly IRepository<Payrolls> _repository;
        private readonly IRepository<Allowance> _allowanceRepository;
        private readonly IRepository<Deduction> _deductionRepository;

        public PayrollController(PayrollService payrollService, IRepository<Payrolls> repository,IRepository<Allowance> allowanceRepository,IRepository<Deduction> deductionRepository)
        {

            _payrollService = payrollService;
            _repository = repository;
            _allowanceRepository = allowanceRepository;
            _deductionRepository = deductionRepository;
           

        }







        [HttpGet("calculate-dynamic/{employeeId}")]
        public async Task<IActionResult> CalculateSalaryPayroll([FromQuery] Guid categoryId, [FromRoute] Guid employeeId, [FromQuery] int? YearOfService)
        {
            var salary = await _payrollService.CalculateSalaryAsPerScale(categoryId, employeeId, YearOfService);
            return Ok(new { result = salary });
        }


        [HttpPost("{employeeId}")]

        public async Task<IActionResult> CalculatePayrollAsPerSalaryScale(
            [FromQuery] Guid categoryId,
            [FromRoute] Guid employeeId, 
            [FromQuery] int? YearOfService, 
            [FromBody] CreatePayrollDto createPayrollDto)
        {
            var baseSalary = await _payrollService.CalculateSalaryAsPerScale(categoryId, employeeId, YearOfService);


            var deductions = await _deductionRepository.GetAll(d => d.EmployeeId == employeeId);
            var totalDeductions = deductions.Sum(d => d.Amount);

            var allowances = await _allowanceRepository.GetAll(d => d.EmployeeId == employeeId);
            var totalAllowances = allowances.Sum(d => d.Amount);

            var payroll = new Payrolls
            {
                Allowances = totalAllowances,
                BasicSalary = baseSalary,
                Deductions = totalDeductions,
                PayDate = new DateTime(),
                EmployeeId = employeeId,
                NetPay = baseSalary + totalAllowances - totalDeductions
            };

            await _repository.AddAsync(payroll);
            await _repository.SaveChangesAsync();

            return Ok(new {isSuccess = true , message = "Successfully created entries for payroll calculation"});

        }

        [HttpGet("{id}")]
            public async Task<IActionResult> GetById(Guid id)
        {
            var  payroll =  _repository.Get(u => u.Id == id
           //includeProperties: "Employees"
                );

            if (payroll == null)
            {
                return NotFound(new { isSuccess = false, message = "Not found" });
            }

            var response = new
            {
                id = payroll.Id,
                payroll.NetPay,
                payroll.PayDate,
                payroll.BasicSalary,
                payroll.Deductions,

                //employees = payroll.Employee?.Select(e => new
                //{
                //    id = e.Id,
                //    firstName = e.FirstName,
                //    lastName = e.LastName,
                //    email = e.Email,
                //    currentSalary = e.CurrentSalary,
                //    yearsOfService = e.YearsOfService
                //}),
            };


            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayroll()
        {

            var result = await _repository.GetAll();
            return Ok(new {data =result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdatePayrollDto Dto, Guid Id)
        {

            var payroll = await _repository.FindByIdAsync(Id);

            if (payroll == null)
            {
                return Ok(new { isSuccess = true, Message = "payroll data not found" });
            }

            payroll.NetPay = Dto.NetPay;
            payroll.PayDate = Dto.PayDate;
            payroll.Deductions = Dto.Deductions;
            payroll.Allowances = Dto.Allowances;
            payroll.BasicSalary = Dto.BasicSalary;


            _repository.Update(payroll);
            await _repository.SaveChangesAsync();
            return NoContent();




        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }


        [HttpGet("deductions/total/{employeeId}")]
        public async Task<IActionResult> GetTotalDeductionsByEmployeeId([FromRoute] Guid employeeId)
        {
            var deductions = await _deductionRepository.GetAll(d => d.EmployeeId == employeeId);
            var totalDeductions = deductions.Sum(d => d.Amount);
            return Ok(new { employeeId, totalDeductions });
        }



        [HttpGet("allowance/total/{employeeId}")]
        public async Task<IActionResult> GetTotalAllowancesByEmployeeId([FromRoute] Guid employeeId)
        {
            var allowances = await _allowanceRepository.GetAll(d => d.EmployeeId == employeeId);
            var totalAllowances = allowances.Sum(d => d.Amount);
            return Ok(new { employeeId, totalAllowances });
        }








    }
}
