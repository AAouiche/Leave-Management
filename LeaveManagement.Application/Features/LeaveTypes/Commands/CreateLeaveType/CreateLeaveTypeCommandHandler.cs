using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Application.Logging;


namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, Result<Unit>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;

        public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository , IAppLogger<CreateLeaveTypeCommandHandler> logger)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                _logger.LogWarning("Validation failed: Name is required for creating a new leave type.");
                return Result.Failure<Unit>(new Error("ValidationError", "Name is required."));
            }

            
            if (await _leaveTypeRepository.CheckExistingName(command.Name))
            {
                _logger.LogWarning("Validation failed: Name exists.");
                return Result.Failure<Unit>(LeaveTypeErrors.NameNotUnique);
            }

            var leaveType = new LeaveType
            {
                Name = command.Name,
                Days = command.Days
            };

            await _leaveTypeRepository.CreateAsync(leaveType);
            

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
