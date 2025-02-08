public interface ILeaveRequestRepository 
{

    Task<List<LeaveRequest>> GetAllAsync();
    Task<List<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId);
    Task<LeaveRequest> GetByIdAsync(Guid id);
    Task<LeaveRequest> CreateAsync(LeaveRequest leaveRequest);
    Task UpdateAsync(LeaveRequest leaveRequest);
    Task DeleteAsync(Guid id);
    Task<bool> Exists(Guid id);
    Task ChangeApprovalStatus(Guid leaveRequestId, bool approved);
    
}