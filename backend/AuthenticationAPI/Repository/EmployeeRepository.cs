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

        public EmployeeRepository(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<UserDetailDto>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("EMPLOYEE");
            var userDetailDtos = await Task.WhenAll(employees.Select(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new UserDetailDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList(),
                    AppUserId = user.Id,

                };
            }));
            return userDetailDtos.ToList();
        }


        public async Task<UserDetailDto> GetEmployee(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
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
            // var alreadyExist = await Exists(employeeGuid);
            // if (alreadyExist) return Results.BadRequest(new { isSuccess = false, message = "Employee already exists." });

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

                // Create user with default password "Employee@123"
                var result = await _userManager.CreateAsync(appUser, "Employee@123");
                if (!result.Succeeded)
                {
                    return Results.BadRequest(new { isSuccess = false, message = "Failed to create user account", errors = result.Errors });
                }

                // Assign EMPLOYEE role
                await _userManager.AddToRoleAsync(appUser, "EMPLOYEE");

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
                    DateOfJoining = DateTime.UtcNow
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(new { 
                    isSuccess = true, 
                    message = "Employee created successfully",
                    employeeDetails = new {
                        email = empDto.Email,
                        defaultPassword = "Employee@123"
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest(new { isSuccess = false, message = "Failed to create employee", error = ex.Message });
            }
        }
         

    public async Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotFoundObjectResult(new { isSuccess = false, message = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return new OkObjectResult(new { isSuccess = true, message = "Password changed successfully" });
            }

            return new BadRequestObjectResult(new { isSuccess = false, message = "Failed to change password", errors = result.Errors });
        }
    }
}
