using LeaveManagement.Domain.Authentication;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Identity;
using LeaveManagement.Domain.Models.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.Authentication.Commands
{
    public class LoginCommand : IRequest<Result<UserDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
