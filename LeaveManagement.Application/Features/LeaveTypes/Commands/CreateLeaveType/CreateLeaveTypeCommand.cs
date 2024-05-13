using LeaveManagement.Domain.Common;
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
        public CreateLeaveTypeCommand(CreateLeaveTypeCommand leaveType)
        {
            Name = leaveType.Name;
            Days = leaveType.Days;
        }
        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}
