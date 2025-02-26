
using AuthenticationAPI.Service;

public interface ILeaveAllocationService
    {
        Task<CreateLeaveAllocationDto> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period,Guid appuserId,Guid employeeId);
        Task<bool> UpdateEmployeeAllocation(UpdateLeaveAllocationDtos UpdateDtos);
        Task<LeaveAllocationDto> GetEmployeeAllocation(int employeeId);

        Task<List<LeaveAllocationDto>> GetEmployeeAllocations(string employeeId);
    } 

    

   

