
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(LRDataBaseContext context) : base(context)
        {
        }

        public async Task<List<LeaveRequest>> GetAllWithDetailsAsync()
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllByUserWithDetailsAsync(string userId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.RequestingEmployeeId == userId)
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .ToListAsync();
        }

        public async Task<LeaveRequest> GetByIdWithDetailsAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }
    }
}
