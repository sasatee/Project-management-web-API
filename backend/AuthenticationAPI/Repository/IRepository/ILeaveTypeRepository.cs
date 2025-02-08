namespace AuthenticationAPI.IRepository.IRepository
{
    public interface ILeaveTypeRepository
    {

        Task<LeaveType> GetByIdAsync(Guid id);
        Task<LeaveType> CreateAsync(LeaveType LeaveType);
        Task UpdateAsync(LeaveType leaveType);
        Task DeleteAsync(Guid id);


    }
}
