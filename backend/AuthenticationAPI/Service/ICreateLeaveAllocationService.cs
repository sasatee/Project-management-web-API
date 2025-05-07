
using AuthenticationAPI.DTOs;

public interface ICreateLeaveAllocationService
    {
        Task<CreateLeaveAllocationDto> CreateLeaveAllocationsForYear(Guid leaveTypeId, int period,Guid appuserId, Guid employeeId);
       
    } 

    

   

