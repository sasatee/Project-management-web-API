using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository.IRepository;
using Microsoft.EntityFrameworkCore;

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

        public  async Task<IEnumerable<LeaveType>> GetAllAsync()
        {
            return await _context.LeaveType.ToListAsync();
        }


        public async Task<LeaveType> FindLeaveType(Guid id)
        {
            

            return await _context.LeaveType.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<LeaveType> GetByIdAsync(Guid id)
        {
            return await _context.LeaveType.FindAsync(id);
        }

      
    }
}
