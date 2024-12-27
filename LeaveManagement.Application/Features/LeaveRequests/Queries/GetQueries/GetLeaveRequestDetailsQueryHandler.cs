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
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler<GetLeaveRequestDetailsQuery, Result<LeaveRequestDetailsDto>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<GetLeaveRequestDetailsQueryHandler> _logger;
        private readonly IAccessUser _accessUser;
        private readonly IMapper _mapper;

        public GetLeaveRequestDetailsQueryHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IAppLogger<GetLeaveRequestDetailsQueryHandler> logger,
            IAccessUser accessUser,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
            _accessUser = accessUser;
            _mapper = mapper;
        }

        public async Task<Result<LeaveRequestDetailsDto>> Handle(GetLeaveRequestDetailsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave request details based on the user's role");

            var role = await _accessUser.GetRole();
            var isAdmin = role == "Admin";

            LeaveRequest leaveRequest;
            if (isAdmin)
            {
                leaveRequest = await _leaveRequestRepository.GetByIdWithDetailsAsync(query.Id);
                _logger.LogInformation($"Fetched leave request ID {query.Id} for all users.");
            }
            else
            {
                
                leaveRequest = await _leaveRequestRepository.GetByIdWithDetailsAsync(query.Id);
                _logger.LogInformation($"Fetched leave request ID {query.Id}.");
            }

            if (leaveRequest == null)
            {
                _logger.LogWarning($"Leave request ID {query.Id} not found.");
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.NotFound(query.Id));
            }

            var mappedLeaveRequest = _mapper.Map<LeaveRequestDetailsDto>(leaveRequest);

            _logger.LogInformation("Successfully fetched and mapped leave request details");
            return Result.Success(mappedLeaveRequest);
        }
    }
}
