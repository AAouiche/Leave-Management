using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails
{
    public class GetLeaveRequestDetailsQuery : IRequest<Result<LeaveRequestDetailsDto>>
    {
        public GetLeaveRequestDetailsQuery(int LeaveRequestId)
        {
            Id = LeaveRequestId;
        }

        public int Id { get; set; }
    }
}
