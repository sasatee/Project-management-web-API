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
            return CreatedAtAction(nameof(GetAttendances),new {EmployeeId = addAttendanceDTO.EmployeeId});
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
        public async Task<IActionResult> GetAttendances()
        {

            var attendances = await _attendanceRepository.GetAll();

            return Ok(attendances);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendence([FromRoute] Guid id, [FromBody] UpdateAttencesDtos updateAttencesDtos)
        {
            var attendence = await _attendanceRepository.FindByIdAsync(id);

            if (attendence == null)
            {

                return NotFound(new { isFalse = false, Message = $"attendence not found with {id}" });

            }
            attendence.CheckInTime = updateAttencesDtos.CheckInTime;
            attendence.CheckOutTime = updateAttencesDtos.CheckOutTime;
            attendence.OvertimeHours = updateAttencesDtos.OvertimeHours;
            attendence.Date = updateAttencesDtos.Date;

            _attendanceRepository.Update(attendence);
            await _attendanceRepository.SaveChangesAsync();

            return NoContent();

        }
       [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleAttendece([FromRoute] Guid id)
        {
            var exist =  await _attendanceRepository.FindByIdAsync(id);

            if(exist == null)
            {
                return NotFound();

            }

            var attendence =  _attendanceRepository.DeleteAsync(id);
                await _employeeRepository.SaveChangesAsync();

            return NoContent();


        }
    }
}
