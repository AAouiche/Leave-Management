using AutoMapper;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<UpdateLeaveAllocationCommandHandler> _logger;

        public UpdateLeaveAllocationCommandHandler(
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IAppLogger<UpdateLeaveAllocationCommandHandler> logger)
        {
            _mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveAllocationRepository = leaveAllocationRepository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Validating the update leave allocation request for ID {request.Id}.");

            var validator = new UpdateLeaveAllocationCommandValidator(_leaveTypeRepository, _leaveAllocationRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                _logger.LogWarning($"Validation failed for update leave allocation request for ID {request.Id}.");
                return Result.Failure<Unit>(LeaveAllocationErrors.ValidationFailure(validationResult.Errors));
            }

            var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
            if (leaveAllocation == null)
            {
                _logger.LogWarning($"Leave allocation with ID {request.Id} not found.");
                return Result.Failure<Unit>(LeaveAllocationErrors.NotFound(request.Id));
            }

            _mapper.Map(request, leaveAllocation);

            await _leaveAllocationRepository.UpdateAsync(leaveAllocation);

            _logger.LogInformation($"Leave allocation with ID {request.Id} updated successfully.");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}

