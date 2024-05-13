using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommand : IRequest<Result<int>>
    {
        public UpdateLeaveTypeCommand(LeaveType leaveType)
        {
            Id = leaveType.Id;
            Name = leaveType.Name;
            Days = leaveType.Days;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}
