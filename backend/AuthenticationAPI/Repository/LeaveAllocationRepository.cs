
using AuthenticationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveAllocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveAllocation> CreateAsync(LeaveAllocation leaveAllocation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveAllocation>> GetAllAsync()
        {
            return await _context.LeaveAllocations
                .Include(q=>q.LeaveType)
                .ToListAsync();
        }

        public Task<LeaveAllocation> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveAllocation>> GetLeaveAllocationsByEmployee(string employeeId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LeaveAllocation leaveAllocation)
        {
            throw new NotImplementedException();
        }
    }
}
