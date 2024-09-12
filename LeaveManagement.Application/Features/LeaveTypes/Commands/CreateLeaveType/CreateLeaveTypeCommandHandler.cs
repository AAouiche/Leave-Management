using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Application.Logging;
using FluentValidation;


namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, Result<Unit>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;
        private readonly IValidator<CreateLeaveTypeCommand> _validator;

        public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveTypeCommandHandler> logger, IValidator<CreateLeaveTypeCommand> validator)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                _logger.LogWarning("Validation failed for command: {Command}", command);
                return Result.Failure<Unit>(LeaveTypeErrors.ValidationError(errors));
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

            _logger.LogInformation("LeaveType '{Name}' created successfully.", command.Name);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
