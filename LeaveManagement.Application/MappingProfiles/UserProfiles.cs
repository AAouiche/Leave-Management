using AutoMapper;
using LeaveManagement.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.Ignore()) 
            .AfterMap(async (src, dest, ctx) =>
            {
                var userManager = ctx.Items["UserManager"] as UserManager<ApplicationUser>;
                var roles = await userManager.GetRolesAsync(src);
                dest.Role = roles.FirstOrDefault();
            });
        }
    }
}
