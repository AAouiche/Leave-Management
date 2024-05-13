using AutoMapper;
using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsQueryHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, Result<LeaveAllocationDetailsDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public GetLeaveAllocationDetailsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        public async Task<Result<LeaveAllocationDetailsDto>> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);
            if (leaveAllocation == null)
            {
                return Result.Failure<LeaveAllocationDetailsDto>(LeaveAllocationErrors.NotFound(request.Id));
            }

            var mappedResult = _mapper.Map<LeaveAllocationDetailsDto>(leaveAllocation);
            if (mappedResult == null)
            {
                return Result.Failure<LeaveAllocationDetailsDto>(LeaveAllocationErrors.MappingError());
            }

            return Result<LeaveAllocationDetailsDto>.Success(mappedResult);
        }
    }
}
