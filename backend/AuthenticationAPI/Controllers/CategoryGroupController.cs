using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryGroupController : Controller
    {
        private readonly IRepository<CategoryGroup> _categoryGroupRepository;
        public CategoryGroupController(IRepository<CategoryGroup> categoryGroupRepository)
        {
            _categoryGroupRepository = categoryGroupRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryGroupbyId([FromRoute] Guid id)
        {
            var category = _categoryGroupRepository.Get(
                u => u.Id == id,
                includeProperties: "SalaryProgressions,Employees"
            );

            if (category == null)
            {   
                return NotFound(new { message = "category group not found" });
            }

            var response = new
            {
                id = category.Id,
                name = category.Name,
                salaryProgressions = category.SalaryProgressions?.Select(sp => new
                {
                    year = sp.Year,
                    salary = sp.Salary,
                    increment = sp.Increment
                }),
                employees = category.Employees?.Select(e => new
                {
                    id = e.Id,
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    email = e.Email,
                    currentSalary = e.CurrentSalary,
                    yearsOfService = e.YearsOfService
                })
            };

            return Ok(response);
        }

    

   }

}
