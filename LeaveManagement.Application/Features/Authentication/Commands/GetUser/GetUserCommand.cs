﻿using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Dtos;

namespace LeaveManagement.Application.Features.Authentication.Commands.GetUser
{
    public class GetUserCommand : IRequest<Result<UserDto>>
    {
    }
}
