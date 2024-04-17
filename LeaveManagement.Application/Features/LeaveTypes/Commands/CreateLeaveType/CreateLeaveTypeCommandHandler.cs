using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;


namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, Result<Unit>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            // Validation for empty name
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result.Failure<Unit>(new Error("ValidationError", "Name is required."));
            }

            
            if (await _leaveTypeRepository.CheckExistingName(command.Name))
            {
                return Result.Failure<Unit>(LeaveTypeErrors.NameNotUnique);
            }

            var leaveType = new LeaveType
            {
                Name = command.Name,
                Days = command.Days
            };

            await _leaveTypeRepository.CreateAsync(leaveType);
            

            return Result<Unit>.Success<Unit>(Unit.Value);
        }
    }
}
