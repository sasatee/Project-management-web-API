
using AuthenticationAPI.Service;

public interface ILeaveAllocationService
    {
        Task<bool> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period,Guid appuserId);
        Task<bool> UpdateEmployeeAllocation(UpdateLeaveAllocationDtos UpdateDtos);
        Task<LeaveAllocationDto> GetEmployeeAllocation(int employeeId);

        Task<List<LeaveAllocationDto>> GetEmployeeAllocations(string employeeId);
    } 

    

   

