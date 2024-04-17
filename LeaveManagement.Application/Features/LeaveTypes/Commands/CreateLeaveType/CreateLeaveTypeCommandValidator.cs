using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
    {
        public CreateLeaveTypeCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.Days)
                .GreaterThan(0).WithMessage("Days must be greater than zero.")
                .LessThanOrEqualTo(365).WithMessage("Days cannot be more than 365.");
        }
        
    }
}
