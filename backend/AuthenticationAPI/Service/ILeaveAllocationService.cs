
using AuthenticationAPI.DTOs;

public interface ILeaveAllocationService
    {
        Task<CreateLeaveAllocationDto> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period,Guid appuserId, Guid employeeId);
       
    } 

    

   

