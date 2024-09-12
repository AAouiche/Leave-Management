using LeaveManagement.Application.Features.LeaveTypes.Dtos;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommand : IRequest<Result<Unit>>
    {
        
        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}
