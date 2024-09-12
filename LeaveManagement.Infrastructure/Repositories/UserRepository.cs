using Identity.Authentication;
using Identity.Interfaces;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LRDataBaseContext _context;

        public UserRepository(LRDataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }
    }
}
