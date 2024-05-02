using LeaveManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommand : IRequest<Result<int>>
    {
        public DeleteLeaveTypeCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
