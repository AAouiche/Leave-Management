using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Repositories;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(LRDataBaseContext context) : base(context)
        {
        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await _context.AddRangeAsync(allocations);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AllocationExists(int userId, int leaveTypeId, int period)
        {
            return await _context.LeaveAllocation.AnyAsync(q => q.Id == userId
                                        && q.LeaveTypeId == leaveTypeId
                                        && q.Period == period);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leaveAllocations = await _context.LeaveAllocation
               .Include(q => q.LeaveType)
               .ToListAsync();
            return leaveAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(int userId)
        {
            var leaveAllocations = await _context.LeaveAllocation.Where(q => q.Id == userId)
               .Include(q => q.LeaveType)
               .ToListAsync();
            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await _context.LeaveAllocation
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == id);

            return leaveAllocation;
        }

        public async Task<LeaveAllocation> GetUserAllocations(int userId, int leaveTypeId)
        {
            return await _context.LeaveAllocation.FirstOrDefaultAsync(q => q.Id == userId
                                        && q.LeaveTypeId == leaveTypeId);
        }
    }
}
