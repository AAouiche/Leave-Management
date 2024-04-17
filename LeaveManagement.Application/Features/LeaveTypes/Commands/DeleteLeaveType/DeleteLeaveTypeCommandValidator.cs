using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandValidator : AbstractValidator<DeleteLeaveTypeCommand>
    {
        public DeleteLeaveTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid ID. ID must be greater than zero.");
        }
    }
}
