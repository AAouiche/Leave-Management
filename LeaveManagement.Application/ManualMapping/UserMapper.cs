using Identity.Authentication;
using Identity.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.ManualMapping
{
    public static class UserMapper
    {
        public static async Task<UserDto> MapToUserDto(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Retrieve roles asynchronously
            var roles = await userManager.GetRolesAsync(user);

            // Manually map ApplicationUser to UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = roles.FirstOrDefault(), // Assign the first role found (if any)
                
            };

            return userDto;
        }
    }
}
