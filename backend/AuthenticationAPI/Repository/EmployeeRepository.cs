using AuthenticationAPI.Data;
using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Model;
using System.Linq;

namespace AuthenticationAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeRepository(UserManager<AppUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<GetUserEmployeeDto>> GetEmployees()
        {
            var roleAsEmployee = await _userManager.GetUsersInRoleAsync("EMPLOYEE");
            var GetUserEmployeeDto = new List<GetUserEmployeeDto>();

            foreach (var user in roleAsEmployee)
            {
                var employee = await _context.Employees
                    .Where(e => e.AppUserId == user.Id)
                    .FirstOrDefaultAsync();

                var roles = await _userManager.GetRolesAsync(user);
                GetUserEmployeeDto.Add(new GetUserEmployeeDto
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList(),
                    AppUserId = user.Id,
                    EmployeeId = employee?.Id
                });
            }

            return GetUserEmployeeDto;
        }


        public async Task<UserDetailDto> GetEmployee(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> Exists(string employeeGuid)
        {
            var employee = await _context.Users.FindAsync(employeeGuid);
            return employee != null;
        }

        public async Task<IResult> CreateEmployee(string employeeGuid, EmployeeDto empDto)
        {
            // Check if user already exists
            var alreadyExist = await Exists(employeeGuid);
            if (alreadyExist) return Results.BadRequest(new { isSuccess = false, message = "Employee already exists." });

            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // First verify that the Department exists
                var department = await _context.Departments.FindAsync(empDto.DepartmentId);
                if (department == null)
                {
                    return Results.BadRequest(new { isSuccess = false, message = "Invalid Department ID" });
                }

                // Verify that the JobTitle exists
                var jobTitle = await _context.JobTitles.FindAsync(empDto.JobTitleId);
                if (jobTitle == null)
                {
                    return Results.BadRequest(new { isSuccess = false, message = "Invalid Job Title ID" });
                }

                // Create AppUser first
                var appUser = new AppUser
                {
                    UserName = empDto.Email,
                    Email = empDto.Email,
                    FirstName = empDto.FirstName,
                    LastName = empDto.LastName,
                    PhoneNumber = empDto.Phone,
                    DateJoined = DateTime.UtcNow
                };
                var setDefaultPasswordForEmployee = _configuration.GetSection("Employee").GetSection("EmployeeDefaultPassword").Value!;
                // Create user with default password 
                var result = await _userManager.CreateAsync(appUser, setDefaultPasswordForEmployee);
                if (!result.Succeeded)
                {
                    return Results.BadRequest(new { isSuccess = false, message = "Failed to create user account", errors = result.Errors });
                }

                var setDefaultRoleAsEmployee = _configuration.GetSection("Employee").GetSection("EmployeeRole").Value!;
                // Assign EMPLOYEE role
                await _userManager.AddToRoleAsync(appUser, setDefaultRoleAsEmployee);

                // Create Employee record
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = empDto.FirstName,
                    LastName = empDto.LastName,
                    Email = empDto.Email,
                    Phone = empDto.Phone,
                    Address = empDto.Address,
                    DepartmentId = empDto.DepartmentId,
                    JobTitleId = empDto.JobTitleId,
                    AppUserId = appUser.Id,
                    DateOfJoining = DateTime.UtcNow,
                    // DateOfLeaving = DateTime.UtcNow,
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(new
                {
                    isSuccess = true,
                    message = "Employee created successfully",
                    employeeLoginCredential = new
                    {
                        email = empDto.Email,
                        defaultPassword = $"{setDefaultPasswordForEmployee}"
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest(new { isSuccess = false, message = "Failed to create employee", error = ex.Message });
            }
        }


    }
}
