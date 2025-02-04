
using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository;

using Microsoft.EntityFrameworkCore;


namespace AuthenticationAPI.IRepository.Repository
{
    public class LeaveRequestAllocationRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveRequestAllocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task ChangeApprovalStatus(int leaveRequestId, bool approved)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveRequest> CreateAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveRequest>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveRequest> GetByIdAsync(int id)
        {


            return await _context.LeaveRequests
            .Include(q => q.LeaveType)
            .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<List<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LeaveRequest leaveRequest)
        {
            throw new NotImplementedException();
        }
    }
}
