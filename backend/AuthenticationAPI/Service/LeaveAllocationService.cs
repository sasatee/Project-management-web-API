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

         public async Task<CreateLeaveAllocationDto> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period, Guid appuserId,Guid employeeId)
        {
            try 
            {
                var leaveType = await _leaveTypeRepository.GetByIdAsync(new Guid(leaveTypeId.ToString()));
                if (leaveType == null)
                    throw new Exception("Leave Type not found");

                var appuser = await _context.Users.FindAsync(appuserId.ToString());
                if (appuser == null)
                    throw new Exception("Appuser not found");


                //var employeeUser = await _context.Employees.FindAsync(employeeId.ToString());
                //if (employeeUser == null)
                //    throw new Exception("employee not found");






                var employees = await _employeeRepository.GetEmployees();
                Console.WriteLine($"Found {employees.Count()} employees to process");
                
                var successCount = 0;
                var skipCount = 0;

                foreach (var employee in employees)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(employee.Id) || !Guid.TryParse(employee.Id, out var employeeGuid))
                        {
                            Console.WriteLine($"Skipping employee with invalid ID: {employee.Id}");
                            skipCount++;
                            continue;
                        }

                        // Check if employee exists in Users table
                        var employeeUser = await _context.Users.FindAsync(employeeGuid.ToString());
                        if (employeeUser == null)
                        {
                            Console.WriteLine($"Skipping employee not found in Users: {employeeGuid}");
                            skipCount++;
                            continue;
                        }

                        // Check if employee exists in Employees table
                        var employeeExists = await _context.Employees.FindAsync(employeeGuid);
                        if (employeeExists == null)
                        {
                            Console.WriteLine($"Skipping employee not found in Employees table: {employeeGuid}");
                            skipCount++;
                            continue;
                        }

                        var allocationExists = await _leaveAllocationRepository.AllocationExists(employeeGuid, leaveType.Id, period, appuserId);
                        if (allocationExists)
                        {
                            Console.WriteLine($"Skipping existing allocation for employee: {employeeGuid}");
                            skipCount++;
                            continue;
                        }

                        var allocation = new LeaveAllocation
                        {
                            Id = Guid.NewGuid(),
                            EmployeeId = employeeGuid,
                            LeaveTypeId = leaveType.Id,
                            NumberOfDays = leaveType.DefaultDays,
                            Period = period,
                            DateCreated = DateTime.UtcNow,
                            AppUserId = appuserId.ToString()
                        };

                        Console.WriteLine($"Creating allocation for employee: {employeeGuid}");
                        await _leaveAllocationRepository.CreateAsync(allocation);
                        await _context.SaveChangesAsync();
                        successCount++;
                        Console.WriteLine($"Successfully created allocation for employee: {employeeGuid}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to create allocation for employee {employee.Id}: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        }
                    }
                }

                Console.WriteLine($"Process completed: {successCount} allocations created, {skipCount} skipped");

                return new CreateLeaveAllocationDto
                {
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period,
                    EmployeeId = appuserId.ToString(),
                    LeaveTypeId = leaveType.Id
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Process failed with error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw new Exception($"Failed to create leave allocations: {ex.Message}", ex);
            }
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

   

