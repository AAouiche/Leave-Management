using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }

        public bool Approved { get; set; }
    }
}
