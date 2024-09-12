using LeaveManagement.Shared.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes
{
    public class GetLeaveTypesQuery : IRequest<Result<List<LeaveTypeDto>>>
    {

    }
}
