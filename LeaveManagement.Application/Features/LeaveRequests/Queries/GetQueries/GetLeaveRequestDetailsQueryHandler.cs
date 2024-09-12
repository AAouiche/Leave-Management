using AutoMapper;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Application.Logging;

using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Interfaces;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries
{
    public class GetAllLeaveRequestQueryHandler : IRequestHandler<GetAllLeaveRequestQuery, Result<List<LeaveRequestListDto>>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<GetAllLeaveRequestQueryHandler> _logger;
        private readonly IAccessUser _accessUser;
        private readonly IMapper _mapper;

        public GetAllLeaveRequestQueryHandler(ILeaveRequestRepository leaveRequestRepository, IAppLogger<GetAllLeaveRequestQueryHandler> logger, IAccessUser accessUser, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
            _accessUser = accessUser;
            _mapper = mapper;
        }

        public async Task<Result<List<LeaveRequestListDto>>> Handle(GetAllLeaveRequestQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave requests based on the user's role");

            
            var role = await _accessUser.GetRole();
            var isAdmin = role == "Admin";

            
            List<LeaveRequest> leaveRequests;
            if (isAdmin)
            {
                leaveRequests = await _leaveRequestRepository.GetAllWithDetailsAsync();
                _logger.LogInformation($"Fetched {leaveRequests.Count} leave requests for all users.");
            }
            else
            {
                var userId = _accessUser.GetUserId();
                leaveRequests = await _leaveRequestRepository.GetAllByUserWithDetailsAsync(userId);
                _logger.LogInformation($"Fetched {leaveRequests.Count} leave requests for user ID {userId}.");
            }

            
            var mappedLeaveRequests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

            _logger.LogInformation("Successfully fetched and mapped leave requests");
            return Result.Success(mappedLeaveRequests);
        }
    }
}
