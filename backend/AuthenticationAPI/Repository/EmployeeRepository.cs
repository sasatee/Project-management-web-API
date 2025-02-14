using AuthenticationAPI.Data;
using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AuthenticationAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(UserManager<AppUser> userManager,ApplicationDbContext context) 
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
                    AppUserId = user.Id ,
                    
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
    }
}
