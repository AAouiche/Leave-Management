using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommand : IRequest<Result<Unit>>
    {
        public DeleteLeaveRequestCommand(int leaveRequestId)
        {
            Id = leaveRequestId;
        }

        public int Id { get; private set; }
    }
}
