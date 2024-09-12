using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries
{
    public class GetAllLeaveRequestQuery:IRequest<Result<List<LeaveRequestListDto>>>
    {
    }
}
