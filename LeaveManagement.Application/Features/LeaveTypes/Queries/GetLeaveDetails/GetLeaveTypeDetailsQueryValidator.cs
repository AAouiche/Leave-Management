using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails
{
    public class GetLeaveTypesDetailsQueryValidator : AbstractValidator<GetLeaveTypesDetailsQuery>
    {
        public GetLeaveTypesDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid ID. ID must be greater than zero.");
        }
    }
}
