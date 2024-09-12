using LeaveManagement.Application.Features.LeaveRequests.Base;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommand : BaseLeaveRequest, IRequest<Result<Unit>>
    {
        public string RequestComments { get; set; } = string.Empty;
        public IFormFile? File { get; set; } 
    }
}
