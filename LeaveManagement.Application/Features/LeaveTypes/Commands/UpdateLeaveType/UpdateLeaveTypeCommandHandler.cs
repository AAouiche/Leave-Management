using FluentValidation;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Result<int>>
    {
        private readonly IGenericRepository<LeaveType> _repository;
        private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger;
        private readonly IValidator<UpdateLeaveTypeCommand> _validator;

        public UpdateLeaveTypeCommandHandler(
            IGenericRepository<LeaveType> repository,
            IAppLogger<UpdateLeaveTypeCommandHandler> logger,
            IValidator<UpdateLeaveTypeCommand> validator)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int>> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                _logger.LogWarning("Validation failed for command: {Command}", request);
                return Result.Failure<int>(LeaveTypeErrors.ValidationError(errors));
            }

            var leaveType = await _repository.GetByIdAsync(request.Id);
            if (leaveType == null)
            {
                _logger.LogWarning($"LeaveType with Id {request.Id} not found.");
                return Result.Failure<int>(LeaveTypeErrors.NotFound(request.Id));
            }

            leaveType.Name = request.Name;
            leaveType.Days = request.Days;

            await _repository.UpdateAsync(leaveType);

            _logger.LogInformation($"LeaveType with Id {leaveType.Id} updated successfully.");
            return Result<int>.Success(leaveType.Id);
        }
    }
}
