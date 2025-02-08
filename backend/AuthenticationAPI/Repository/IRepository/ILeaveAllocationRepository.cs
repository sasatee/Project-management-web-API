public interface ILeaveAllocationRepository
{
    Task<List<LeaveAllocation>> GetAllAsync();
    Task<List<LeaveAllocation>> GetLeaveAllocationsByEmployee(string employeeId);
    Task<LeaveAllocation> GetByIdAsync(int id);
    Task<LeaveAllocation> CreateAsync(LeaveAllocation leaveAllocation);
    Task UpdateAsync(LeaveAllocation leaveAllocation);
    Task DeleteAsync(int id);
    Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period);
} 