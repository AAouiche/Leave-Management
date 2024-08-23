using LeaveManagement.Domain.Identity;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Security
{
    public class AccessUser : IAccessUser
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly LRDataBaseContext _context;
        public AccessUser(IHttpContextAccessor HttpContextAccessor, LRDataBaseContext context)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _context = context;
        }
        public string GetUserId()
        {

            var userIdString = _HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {

                throw new InvalidOperationException("User is not authenticated");
            }

            return userIdString;
        }
        public string GetUsername()
        {
            var username = _HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            if (username != null)
                return username;


            throw new InvalidOperationException("Username not found in claims");
        }

        public async Task<ApplicationUser> GetUser()
        {
            var userIdString = _HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {

                throw new InvalidOperationException("User is not authenticated");
            }
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userIdString);
        }
    }
}
