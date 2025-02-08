using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository.IRepository;

namespace AuthenticationAPI.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveTypeRepository(ApplicationDbContext context)
        {
            _context = context;


        }



        public async Task<LeaveType> CreateAsync(LeaveType LeaveType)
        {

            await _context.LeaveType.AddAsync(LeaveType);
            await _context.SaveChangesAsync();
            return LeaveType;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveType> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LeaveType leaveType)
        {
            throw new NotImplementedException();
        }
    }
}
