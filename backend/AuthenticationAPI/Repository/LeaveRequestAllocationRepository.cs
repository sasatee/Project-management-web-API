using AuthenticationAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationAPI.Repository
{
    public class LeaveRequestAllocationRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveRequestAllocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task ChangeApprovalStatus(Guid leaveRequestId, bool approved)
        {
            throw new NotImplementedException();
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

        public Task<bool> Exists(Guid id)
        {
            throw new NotImplementedException();
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
            return await _context.LeaveRequests
                .Where(q => q.RequestingEmployeeId.ToString() == employeeId)
                .Include(q => q.LeaveType)
                .ToListAsync();
        }

        public Task UpdateAsync(LeaveRequest leaveRequest)
        {
            throw new NotImplementedException();
        }
    }
}
