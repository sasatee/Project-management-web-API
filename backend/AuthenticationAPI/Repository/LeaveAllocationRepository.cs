
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

        public async Task<bool> AllocationExists(Guid employeeId, Guid leaveTypeId, int period, Guid appuserId)
        {
               return await _context.LeaveAllocations
                .AnyAsync(
                x => x.EmployeeId == employeeId
                && x.AppUserId == appuserId.ToString()
                && x.LeaveTypeId == leaveTypeId
                && x.Period == period);
        }

        public async Task<LeaveAllocation> CreateAsync(LeaveAllocation leaveAllocation)
        {
               await _context.LeaveAllocations.AddAsync(leaveAllocation);
               await _context.SaveChangesAsync();
               return leaveAllocation;
        }

        public async Task DeleteAsync(Guid id)
        {
                var leaveAllocation = await GetByIdAsync(id);
                _context.LeaveAllocations.Remove(leaveAllocation);
                await _context.SaveChangesAsync();


        }

        public async Task<List<LeaveAllocation>> GetAllAsync()
        {
                     return await _context.LeaveAllocations
                    .Include(q=>q.LeaveType)
                    .ToListAsync();
        }

        public async Task<LeaveAllocation> GetByIdAsync(Guid id)
        {
            return await _context.LeaveAllocations.FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw new InvalidOperationException("LeaveAllocation not found");
        }


        public async Task<List<LeaveAllocation>> GetLeaveAllocationsByEmployee(Guid employeeId)
        {
                 return await _context.LeaveAllocations
                .Where(q=>q.EmployeeId == employeeId).Include(q=>q.LeaveType).ToListAsync();
        }

        public Task UpdateAsync(LeaveAllocation leaveAllocation)
        {
                _context.Entry(leaveAllocation).State = EntityState.Modified;
                return _context.SaveChangesAsync();
        }
    }
}
