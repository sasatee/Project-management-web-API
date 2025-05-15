using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class DepartmentController : Controller
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentController(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }



        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] CreateDepartmentDto departmentDto)
        {
            var department = new Department
            {
                //Id = Guid.NewGuid(),
                DepartmentName = departmentDto.DepartmentName,
                HeadOfDepartment = departmentDto.HeadOfDepartment,
            };
            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();
            return Ok(new { AuthResponse = new  { isSuccess = true, Message = "Successfully created department" }, Result = departmentDto });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid id, UpdateCreateDepartmentDto model)
        {
            var department = await _departmentRepository.FindByIdAsync(id);
            Console.WriteLine(department);
            if (department == null)
            {
                return NotFound(new { isSuccess = false, Message = "Department not found" } );
            }

            department.DepartmentName = model.DepartmentName;
            department.HeadOfDepartment = model.HeadOfDepartment;

             _departmentRepository.Update(department);
            await _departmentRepository.SaveChangesAsync();

            return Ok(new  { isSuccess = true, Message = "Successfully updated department", Result = model } );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] Guid id)
        {

            var result  = await _departmentRepository.FindByIdAsync(id);

            return Ok(new { isSuccess = true, Message = "Successfully updated department", Result = result } );

        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {

            var result = await _departmentRepository.GetAll();

            return Ok(new  { isSuccess = true, Message = "Successfully get all department", Result = result });

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentByid(Guid id)
        {
            await _departmentRepository.DeleteAsync(id);
            await _departmentRepository.SaveChangesAsync();
            return Ok(new  { isSuccess = true, Message = "Successfully deleted department" } );
        }
    }
}
