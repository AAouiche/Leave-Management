using AutoMapper;
using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;
using LeaveManagement.Domain.Common;
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

        public GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<LeaveAllocationDto>>> Handle(GetAllLeaveAllocationQuery request, CancellationToken cancellationToken)
        {
            var leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();
            if (leaveAllocations == null || leaveAllocations.Count == 0)
            {
               
                return Result.Failure<List<LeaveAllocationDto>>(LeaveAllocationErrors.NotFoundGeneric());
            }

            var allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
            if (allocations == null || allocations.Count == 0)
            {
                
                return Result.Failure<List<LeaveAllocationDto>>(LeaveAllocationErrors.MappingError());
            }

            return Result<List<LeaveAllocationDto>>.Success(allocations);
        }
    }
}
