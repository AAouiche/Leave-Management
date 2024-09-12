using Identity.Authentication;
using Identity.Dtos;
using Identity.Interfaces;

using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessUser(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new InvalidOperationException("User is not authenticated");
            }

            return userIdString;
        }

        public string GetUsername()
        {
            var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrEmpty(username))
            {
                return username;
            }

            throw new InvalidOperationException("Username not found in claims");
        }

        public async Task<ApplicationUser> GetUser()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            return user;
        }

        public async Task<string> GetRole()
        {
            var user = await GetUser();

            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault() ?? throw new InvalidOperationException("Role not found");
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }
    }
}
