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
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(LRDataBaseContext context) : base(context)
        {
        }

        public async Task<bool> CheckExistingName(string name)
        {
            return await _context.LeaveTypes.AnyAsync(lt => lt.Name == name);
        }
    }
}
