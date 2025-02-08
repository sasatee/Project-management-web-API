using AuthenticationAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationAPI.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task ChangeApprovalStatus(Guid leaveRequestId, bool approved)
        {

            var leaveRequest = await GetByIdAsync(leaveRequestId);
            leaveRequest.Approved = approved;
            await UpdateAsync(leaveRequest);

        }

        public async Task<LeaveRequest> CreateAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.LeaveRequests.AnyAsync(x => x.Id == id);
        }

       
        public async Task<List<LeaveRequest>> GetAllAsync()
        {

            return await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .ToListAsync();

        }

        public async Task<LeaveRequest> GetByIdAsync(Guid id)
        {



            return await _context.LeaveRequests
            .Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId)
        {  
                 var requests = await _context.LeaveRequests
                .Where(q => q.AppUserId.ToString() == employeeId)
                .Include(q => q.LeaveType)
                .ToListAsync();

            return requests;
        }

        public async Task UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.Entry(leaveRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
