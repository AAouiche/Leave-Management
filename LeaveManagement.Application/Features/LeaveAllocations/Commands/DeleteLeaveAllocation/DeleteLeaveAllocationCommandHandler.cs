using AutoMapper;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, Result<Unit>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<DeleteLeaveAllocationCommandHandler> _logger;

        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper, IAppLogger<DeleteLeaveAllocationCommandHandler> logger)
        {
            this._leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Attempting to delete leave allocation with ID {request.Id}.");

            var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
            if (leaveAllocation == null)
            {
                _logger.LogWarning($"Leave allocation with ID {request.Id} not found.");
                return Result.Failure<Unit>(LeaveAllocationErrors.NotFound(request.Id));
            }

            await _leaveAllocationRepository.DeleteAsync(leaveAllocation);

            _logger.LogInformation($"Leave allocation with ID {request.Id} deleted successfully.");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
