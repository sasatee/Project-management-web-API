using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payroll.Model;

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

    //    [HttpGet]
    //    public async Task<IActionResult> GetJobTitle()
    //    {
    //       return Ok(await _categoryGroupRepository.GetAll());
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetJobTitle([FromRoute] Guid id)
    //    {
    //        return Ok(await _categoryGroupRepository.FindByIdAsync(id));
    //    }


    //    [HttpPost]
    //    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto Dtos)
    //    {

    //        var jobTitle = new JobTitle
    //        {
                
    //            Grade = Dtos.Grade,
    //            BaseSalary = Dtos.BaseSalary,
    //            Title = Dtos.Title
    //        };
    //        await _categoryGroupRepository.AddAsync(jobTitle);
    //        await _categoryGroupRepository.SaveChangesAsync();
    //        return NoContent();
    //    }


    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteJobById([FromRoute] Guid id)
    //    {
    //        await _categoryGroupRepository.DeleteAsync(id);
    //        await _categoryGroupRepository.SaveChangesAsync();
    //        return Ok(new { isSuccess = true, Message = "Successfully deleted Job title" });
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateJobById([FromRoute] Guid id,UpdateJobTitledDtos dto)
    //    {
    //        var JobTitle = await _categoryGroupRepository.FindByIdAsync(id);
    //        if(JobTitle == null)
    //        {
    //            return NotFound(new { isSuccess = false, Message = "Job title not found" });
    //        }

    //        JobTitle.Title = dto.Title ;
    //        JobTitle.BaseSalary= dto.BaseSalary;
    //        JobTitle.Grade = dto.Grade;

    //        _categoryGroupRepository.Update(JobTitle);
    //        await _categoryGroupRepository.SaveChangesAsync();
    //        return Ok(new { isSuccess = true, Message = "Successfully updated Job title" });
    //    }

   }

}
