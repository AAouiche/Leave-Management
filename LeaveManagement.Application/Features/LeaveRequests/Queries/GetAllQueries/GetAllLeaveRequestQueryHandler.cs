using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Logging;
using LeaveManagement.Application.MappingProfiles;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries
{
    public class GetAllLeaveRequestQueryHandler : IRequestHandler<GetAllLeaveRequestQuery, Result<List<LeaveRequestListDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<GetAllLeaveRequestQueryHandler> _logger;

        public GetAllLeaveRequestQueryHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository, IAppLogger<GetAllLeaveRequestQueryHandler> logger)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
        }

        public async Task<Result<List<LeaveRequestListDto>>> Handle(GetAllLeaveRequestQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all leave requests");

            var leaveRequests = await _leaveRequestRepository.GetAllAsync();
            var mappedLeaveRequests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

            if (mappedLeaveRequests == null)
            {
                _logger.LogWarning("Mapping failure occurred while fetching leave requests");
                return Result.Failure<List<LeaveRequestListDto>>(LeaveRequestErrors.MappingFailure());
            }

            _logger.LogInformation("Successfully fetched and mapped leave requests");
            return Result.Success(mappedLeaveRequests);
        }
    }
}
