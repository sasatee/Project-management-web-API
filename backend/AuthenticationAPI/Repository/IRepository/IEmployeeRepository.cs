using AuthenticationAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<UserDetailDto>> GetEmployees();
        Task<UserDetailDto> GetEmployee(Guid userId);
        Task<bool> Exists(string employeeGuid);

        Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword);
        Task<IResult> CreateEmployee(string employeeGuid, EmployeeDto empDto);
    }
}
