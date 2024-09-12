
using Identity.Authentication;
using Identity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Identity.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal ValidateToken(TokenDTO token);
    }
}
