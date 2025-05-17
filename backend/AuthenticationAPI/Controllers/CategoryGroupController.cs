using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryGroupController : Controller
    {
        private readonly IRepository<CategoryGroup> _categoryGroupRepository;
        private readonly SeedSalaryForCategory _createCategorySeedService;
        public CategoryGroupController(IRepository<CategoryGroup> categoryGroupRepository, SeedSalaryForCategory createCategorySeedService)
        {
            _categoryGroupRepository = categoryGroupRepository;
            _createCategorySeedService = createCategorySeedService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryGroup()
        {
            var categoryGroup = await _categoryGroupRepository.GetAll();
            return Ok(categoryGroup);
        }

        [HttpGet("/EmployeeWith/SalaryProgression/{id}")]
        public IActionResult GetCategoryGroupbyId([FromRoute] Guid id)
        {
            var category = _categoryGroupRepository.Get(
                u => u.Id == id,
                includeProperties: "SalarySteps,Employees"
            );

            if (category == null)
            {
                return NotFound(new { message = "category group not found" });
            }

            var response = new
            {
                id = category.Id,
                name = category.Name,
                count =  category.Employees?.Count(),

                EmployeeData = category.Employees?.Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.DateOfLeaving,
                    e.DateOfJoining,
                    e.AppUserId,
                    e.Email,
                    e.CurrentSalary,
                    e.Address,
                    e.DepartmentId
                    
                }),
        
                SalarySteps = category.SalarySteps?.Select(s => new
                {
                    Id = s.Id,
                    Increment = s.Increment,
                    ToAmount = s.ToAmount,
                    FromAmount = s.FromAmount,
                    CategoryGroupId = s.CategoryGroupId,
           
                })
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryGroupAsync([FromBody] CreateCategoryGroupDto categoryDto)
        {


            await _createCategorySeedService.CreateCategoryGroup(categoryDto.Name);

            return NoContent();


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryGroupDto updateDto, [FromRoute] Guid id)
        {
           var categoryGroup =  await _categoryGroupRepository.FindByIdAsync(id);

            if(categoryGroup is null)
            {
                return NotFound(new { isSuccess = false, message = "Category group is not found" });

            }
            categoryGroup.Name = updateDto.Name;
            _categoryGroupRepository.Update(categoryGroup);
            await _categoryGroupRepository.SaveChangesAsync();
            return NoContent();



        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var exist = await _categoryGroupRepository.FindByIdAsync(id);

            if (exist != null)
            {
                await _categoryGroupRepository.DeleteAsync(id);
                await _categoryGroupRepository.SaveChangesAsync();
            }
            return NoContent();
        }







    }
}
