using AuthenticationAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Payroll.Model;

namespace AuthenticationAPI.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<GetUserEmployeeDto>> GetEmployees();
        Task<UserDetailDto> GetEmployee(Guid userId);
        Task<bool> Exists(string employeeGuid);


        Task<IResult> CreateEmployee(string employeeGuid, EmployeeDto empDto);
    }
}
