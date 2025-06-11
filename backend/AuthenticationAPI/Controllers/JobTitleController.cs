using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleController : Controller
    {
        private readonly IRepository<JobTitle> _JobRepository;

        public JobTitleController(IRepository<JobTitle> JobRepository)
        {
            _JobRepository = JobRepository;
        }

        [HttpGet("JobAssociateWithEmployee/{id}")]
        public IActionResult GetJobTitleWithEmployeesbyId([FromRoute] Guid id)
        {
            var jobTitle = _JobRepository.Get(u => u.Id == id, includeProperties: "Employees"); // relationship with employee

            if (jobTitle == null)
            {
                return NotFound(new { message = "JobTitle not found" });
            }

            var response = new
            {
                Id = jobTitle.Id,
                Title = jobTitle.Title,
                Employees = jobTitle.Employees?.Select(e => new
                {
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Phone,
                    e.Address,
                    e.DateOfJoining,
                    e.DepartmentId,
                    e.JobTitleId,
                    e.AppUserId
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobTitle()
        {
           return Ok(await _JobRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobTitle([FromRoute] Guid id)
        {
            return Ok(await _JobRepository.FindByIdAsync(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDto Dtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobTitle = new JobTitle
            {
                
              
                Title = Dtos.Title
            };
            await _JobRepository.AddAsync(jobTitle);
            await _JobRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobById([FromRoute] Guid id)
        {
            var jobTitle = await _JobRepository.FindByIdAsync(id);
            if (jobTitle == null)
            {
                return NotFound(new { isSuccess = false, Message = $"Job title not found with {id}" });
            }

            await _JobRepository.DeleteAsync(id);
            await _JobRepository.SaveChangesAsync();
            return Ok(new { isSuccess = true, Message = "Successfully deleted Job title" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobById([FromRoute] Guid id,UpdateJobTitledDtos dto)
        {
            var JobTitle = await _JobRepository.FindByIdAsync(id);
            if(JobTitle == null)
            {
                return NotFound(new { isSuccess = false, Message = "Job title not found" });
            }

            JobTitle.Title = dto.Title ;
          

            _JobRepository.Update(JobTitle);
            await _JobRepository.SaveChangesAsync();
            return Ok(new { isSuccess = true, Message = "Successfully updated Job title" });
        }

    }

}
