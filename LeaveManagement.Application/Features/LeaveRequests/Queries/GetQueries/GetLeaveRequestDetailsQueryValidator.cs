using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Base;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Domain.LeaveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries
{
    public class GetLeaveRequestDetailsQueryValidator : AbstractValidator<GetLeaveRequestDetailsQuery>
    {
        public GetLeaveRequestDetailsQueryValidator()
        {
            RuleFor(x => x.LeaveRequestId)
                .GreaterThan(0).WithMessage("LeaveRequestId must be greater than zero.");
        }
    }
}
