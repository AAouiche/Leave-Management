using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.Authentication.Commands.GetAllUsers
{
    public class GetAllUsersCommand : IRequest<Result<List<UserDto>>>
    {
    }
}
