public interface ILeaveAllocationRepository
{
    Task<List<LeaveAllocation>> GetAllAsync();
    Task<List<LeaveAllocation>> GetLeaveAllocationsByEmployee(Guid employeeId);
    Task<LeaveAllocation> GetByIdAsync(Guid id);
    Task CreateAsync(LeaveAllocation leaveAllocation);
    Task UpdateAsync(LeaveAllocation leaveAllocation);
    Task DeleteAsync(Guid id);
    Task<bool> AllocationExists(Guid employeeId, Guid leaveTypeId, int period,Guid appuserId);
} 