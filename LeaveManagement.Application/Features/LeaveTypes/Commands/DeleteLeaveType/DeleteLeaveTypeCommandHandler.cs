using FluentValidation;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Result<int>>
    {
        private readonly IGenericRepository<LeaveType> _repository;
        private readonly IAppLogger<DeleteLeaveTypeCommandHandler> _logger;
        private readonly IValidator<DeleteLeaveTypeCommand> _validator;

        public DeleteLeaveTypeCommandHandler(
            IGenericRepository<LeaveType> repository,
            IAppLogger<DeleteLeaveTypeCommandHandler> logger,
            IValidator<DeleteLeaveTypeCommand> validator)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<int>> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            
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
                _logger.LogWarning("Validation failed: LeaveType doesn't exist.");
                return Result.Failure<int>(LeaveTypeErrors.NotFound(request.Id));
            }

            await _repository.DeleteAsync(leaveType);
            _logger.LogInformation($"LeaveType with Id {request.Id} deleted successfully.");
            return Result<int>.Success(request.Id);
        }
    }
}
