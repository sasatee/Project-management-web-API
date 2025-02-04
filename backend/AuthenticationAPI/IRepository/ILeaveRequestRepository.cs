public interface ILeaveRequestRepository 
{

    Task<List<LeaveRequest>> GetAllAsync();
    Task<List<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId);
    Task<LeaveRequest> GetByIdAsync(int id);
    Task<LeaveRequest> CreateAsync(LeaveRequest leaveRequest);
    Task UpdateAsync(LeaveRequest leaveRequest);
    Task DeleteAsync(int id);
    Task<bool> Exists(int id);
    Task ChangeApprovalStatus(int leaveRequestId, bool approved);
}