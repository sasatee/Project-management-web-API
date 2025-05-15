using AuthenticationAPI.DTOs;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : Controller
    {

        private readonly  IRepository<Attendance> _attendanceRepository;
        private readonly  IRepository<Employee> _employeeRepository;

        public AttendanceController(IRepository<Attendance> attendanceRepository, IRepository<Employee> employeeRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        public async Task <IActionResult> AddAttendance([FromBody] AddAttendanceDTO addAttendanceDTO)
        {
            //if employee exists
            var employee = _employeeRepository.Get(e => e.Id == addAttendanceDTO.EmployeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {addAttendanceDTO.EmployeeId} not found");
            }

            var attendance = new Attendance()
            {
                CheckInTime = addAttendanceDTO.CheckInTime,
                CheckOutTime = addAttendanceDTO.CheckOutTime,
                Date = addAttendanceDTO.Date,
                OvertimeHours = addAttendanceDTO.OvertimeHours,
                EmployeeId = addAttendanceDTO.EmployeeId,
            };
            await _attendanceRepository.AddAsync(attendance);
            await _attendanceRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendances),new {id = addAttendanceDTO.Id});
        }

        [HttpGet("{id}")]
        public IActionResult GetAttendances([FromRoute] Guid id)
        {

            var attendance =  _attendanceRepository.Get(u => u.Id == id,includeProperties:"Employee");
            if(attendance == null)
            {
                return NotFound();
            }

            var response = new
            {
              attendance.Id, 
              attendance.Date,
              attendance.CheckInTime,
              attendance.CheckOutTime,
              attendance.OvertimeHours,
              employeeId = attendance.Employee.Id,  
               


            };


            return Ok(response);

        }

        [HttpGet]
        public IActionResult GetAttendances()
        {

            var attendances = _attendanceRepository.GetAll();

            return Ok(attendances);
        }
    }

}
