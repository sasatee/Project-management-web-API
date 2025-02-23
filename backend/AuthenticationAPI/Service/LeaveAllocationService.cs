using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository.IRepository;
using AuthenticationAPI.Repository.IRepository;

namespace AuthenticationAPI.Service
{

    public class LeaveAllocationService : ILeaveAllocationService
        {
            private readonly ILeaveAllocationRepository _leaveAllocationRepository;
            private readonly ILeaveTypeRepository _leaveTypeRepository;
            private readonly IEmployeeRepository   _employeeRepository;
            private readonly ApplicationDbContext _context;

            public LeaveAllocationService(ILeaveAllocationRepository leaveAllocationRepository,ILeaveTypeRepository leaveTypeRepositiory,IEmployeeRepository employeeRepository, ApplicationDbContext context)
            {
                _leaveAllocationRepository = leaveAllocationRepository;
                _leaveTypeRepository = leaveTypeRepositiory;
                _employeeRepository = employeeRepository;
                _context = context;
            }

        public async Task<bool> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period, Guid appuserId)
        {
            var leaveType = await _leaveTypeRepository.GetByIdAsync(new Guid(leaveTypeId.ToString()));
            if (leaveType == null)
                throw new Exception("Leave Type not found");


                //Verify Appuser exsists
                var appuser = await _context.Users.FindAsync(appuserId.ToString());
                if (appuser == null)
                    throw new Exception("Appuser not found");



            var employees = await _employeeRepository.GetEmployees();
            var allocations = new List<LeaveAllocation>();

       foreach (var employee in employees)
            {
                if (string.IsNullOrEmpty(employee.Id) || !Guid.TryParse(employee.Id, out var employeeGuid))
                    continue;

                // Check if employee exists in Users table
                var employeeExists = await _context.Users.FindAsync(employeeGuid.ToString());
                if (employeeExists == null)
                    continue;

                if (await _leaveAllocationRepository.AllocationExists(employeeGuid, leaveType.Id, period, appuserId))
                    continue;

                allocations.Add(new LeaveAllocation
                {
                    Id = Guid.NewGuid(), // Ensure ID is set
                    EmployeeId = employeeGuid,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period,
                    DateCreated = DateTime.UtcNow,
                    AppUserId = appuserId.ToString()
                });
        }
            try
            {

                foreach (var allocation in allocations)
                {
                    await _leaveAllocationRepository.CreateAsync(allocation);
                }
              
                Console.WriteLine("Allocation created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create allocation: {ex.Message}");
                throw;
            }
           


            return true;
        }


            public Task<LeaveAllocationDto> GetEmployeeAllocation(int employeeId)
            {
                throw new NotImplementedException();
            }

            public Task<List<LeaveAllocationDto>> GetEmployeeAllocations(string employeeId)
            {
                throw new NotImplementedException();
            }

            public Task<bool> UpdateEmployeeAllocation(UpdateLeaveAllocationDtos UpdateDtos)
            {
                throw new NotImplementedException();
            }
        }

    }

   

