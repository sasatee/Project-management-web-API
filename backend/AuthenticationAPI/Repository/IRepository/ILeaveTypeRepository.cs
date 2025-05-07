namespace AuthenticationAPI.IRepository.IRepository
{
    public interface ILeaveTypeRepository
    {

        Task<LeaveType> GetByIdAsync(Guid id);

        Task<IEnumerable<LeaveType>> GetAllAsync();
        Task<LeaveType> CreateAsync(LeaveType LeaveType);



    }
}
