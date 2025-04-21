using AuthenticationAPI.Data;
using AuthenticationAPI.DTOs;
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

         public async Task<CreateLeaveAllocationDto> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period, Guid appuserId, Guid employeeId)
        {
            try 
            {
                  // Find the leave type
                var leaveType = await _leaveTypeRepository.GetByIdAsync(new Guid(leaveTypeId.ToString()));
                if (leaveType == null)
                    throw new Exception("Leave Type not found");

               // Find the appuser
                var appuser = await _context.Users.FindAsync(appuserId.ToString());
                if (appuser == null)
                    throw new Exception("Appuser not found");

                // Find the employee
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    throw new Exception("Employee not found");

                // Check if employee exists in Users table
                var employeeUser = await _context.Users.FindAsync(employee.AppUserId);
                if (employeeUser == null)
                    throw new Exception("Employee user not found");

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Check if allocation already exists
                        var allocationExists = await _leaveAllocationRepository.AllocationExists(
                            employeeId, leaveType.Id, period, appuserId);
                        
                        if (allocationExists)
                        {
                            throw new Exception("Leave allocation already exists for this employee");
                        }

                        var allocation = new LeaveAllocation
                        {
                            Id = Guid.NewGuid(),
                            EmployeeId = employeeId,
                            LeaveTypeId = leaveType.Id,
                            NumberOfDays = leaveType.DefaultDays,
                            Period = period,
                            DateCreated = DateTime.UtcNow,
                            AppUserId = appuserId.ToString()
                        };

                        await _leaveAllocationRepository.CreateAsync(allocation);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new CreateLeaveAllocationDto
                        {
                            NumberOfDays = leaveType.DefaultDays,
                            Period = period,
                            EmployeeId = employeeId.ToString(),
                            LeaveTypeId = leaveType.Id
                        };
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create leave allocation: {ex.Message}", ex);
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

   

