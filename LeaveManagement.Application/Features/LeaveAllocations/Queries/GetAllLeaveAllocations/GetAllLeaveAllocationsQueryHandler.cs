using AutoMapper;
using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;
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

namespace LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllAllocations
{
    public class GetAllLeaveAllocationsQueryHandler : IRequestHandler<GetAllLeaveAllocationQuery, Result<List<LeaveAllocationDto>>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<GetAllLeaveAllocationsQueryHandler> _logger;

        public GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper, IAppLogger<GetAllLeaveAllocationsQueryHandler> logger)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<LeaveAllocationDto>>> Handle(GetAllLeaveAllocationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all leave allocations with details.");

            var leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();
            if (leaveAllocations == null || leaveAllocations.Count == 0)
            {
                _logger.LogWarning("No leave allocations found.");
                return Result.Failure<List<LeaveAllocationDto>>(LeaveAllocationErrors.NotFoundGeneric());
            }

            var allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
            if (allocations == null || allocations.Count == 0)
            {
                _logger.LogError("Failed to map leave allocations.");
                return Result.Failure<List<LeaveAllocationDto>>(LeaveAllocationErrors.MappingError());
            }

            _logger.LogInformation("Successfully retrieved and mapped leave allocations.");
            return Result<List<LeaveAllocationDto>>.Success(allocations);
        }
    }
}
