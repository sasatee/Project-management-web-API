using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowanceController : Controller
    {

        private readonly IRepository<Allowances> _repository;

        public AllowanceController(IRepository<Allowances> repository)
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
        public async Task<IActionResult> GetAllowanceById([FromRoute] Guid id)
        { 

            var allowances = await _repository.FindByIdAsync(id);


            return Ok(allowances);
        
        
        
        }

      ///TODO : find allowance with exployee
      ///payroll with allowance
      ///update 
      ///delete
      ///create
    }
}
