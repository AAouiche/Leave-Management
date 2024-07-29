using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries
{
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler<GetLeaveRequestDetailsQuery, Result<LeaveRequestDetailsDto>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<Result<LeaveRequestDetailsDto>> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithDetails(request.LeaveRequestId);
            if (leaveRequest == null)
            {
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.NotFound(request.LeaveRequestId));
            }

            var leaveRequestDetails = _mapper.Map<LeaveRequestDetailsDto>(leaveRequest);
            if (leaveRequestDetails == null)
            {
               
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.MappingFailure());
            }

            return Result.Success(leaveRequestDetails);
        }
    }
}
