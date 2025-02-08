using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

   

   
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {

        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        public LeaveAllocationController(ILeaveAllocationRepository repository)
        {
            _leaveAllocationRepository = repository;
            
        }




      
    }
}
