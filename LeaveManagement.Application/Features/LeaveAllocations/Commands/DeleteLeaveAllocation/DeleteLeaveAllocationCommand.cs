using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommand : IRequest<Result<Unit>>
    {
        public DeleteLeaveAllocationCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
