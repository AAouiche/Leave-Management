using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommand : IRequest<Result<Unit>>
    {
        public CancelLeaveRequestCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
