using AuthenticationAPI.DTOs;

namespace AuthenticationAPI.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<UserDetailDto>> GetEmployees();
        Task<UserDetailDto> GetEmployee(Guid userId);
        Task<bool> Exists(string employeeGuid);
    }
}
