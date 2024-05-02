using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Repositories;
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
        private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;

        public DeleteLeaveTypeCommandHandler(IGenericRepository<LeaveType> repository, IAppLogger<CreateLeaveTypeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _repository.GetByIdAsync(request.Id);
            if (leaveType == null)
            {
                _logger.LogWarning("Validation failed: LeaveType doesn't exist.");
                return Result.Failure<int>(LeaveTypeErrors.NotFound(request.Id));
            }

            await _repository.DeleteAsync(leaveType);
            return Result<int>.Success(request.Id);
        }
    }
}
