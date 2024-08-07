using AutoMapper;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveAllocationCommandHandler> _logger;

        public CreateLeaveAllocationCommandHandler(IMapper mapper,
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<CreateLeaveAllocationCommandHandler> logger)
        {
            _mapper = mapper;
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._leaveTypeRepository = leaveTypeRepository;
            this._logger = logger;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Validating the create leave allocation request.");

            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                _logger.LogWarning("Validation failed for create leave allocation request.");
                return Result.Failure<Unit>(LeaveAllocationErrors.ValidationFailure(validationResult.Errors));
            }

            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);
            if (leaveType == null)
            {
                _logger.LogWarning($"Leave type with ID {request.LeaveTypeId} not found.");
                return Result.Failure<Unit>(LeaveAllocationErrors.NotFound(request.LeaveTypeId));
            }

            var leaveAllocation = _mapper.Map<LeaveAllocation>(request);
            await _leaveAllocationRepository.CreateAsync(leaveAllocation);

            _logger.LogInformation($"Leave allocation created successfully for leave type ID {request.LeaveTypeId}.");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
