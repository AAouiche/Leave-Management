
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Application.Logging;
using LeaveManagement.Application.ManualMapping;
using LeaveManagement.Application.MappingProfiles;

using LeaveManagement.Shared.Common;

using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Authentication;
using Identity.Interfaces;
using Identity.Dtos;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries
{
    public class GetAllLeaveRequestQueryHandler : IRequestHandler<GetAllLeaveRequestQuery, Result<List<LeaveRequestListDto>>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<GetAllLeaveRequestQueryHandler> _logger;
        private readonly IAccessUser _accessUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllLeaveRequestQueryHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IAppLogger<GetAllLeaveRequestQueryHandler> logger,
            IAccessUser accessUser,
            UserManager<ApplicationUser> userManager)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
            _accessUser = accessUser;
            _userManager = userManager;
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

            var mappedLeaveRequests = new List<LeaveRequestListDto>();

            foreach (var request in leaveRequests)
            {
                var roles = await _userManager.GetRolesAsync(request.Employee);
                var employeeRole = roles.FirstOrDefault();


                mappedLeaveRequests.Add(new LeaveRequestListDto
                {

                    Id=request.Id,
                    RequestingEmployeeId = request.RequestingEmployeeId,
                    Employee = new UserDto
                    {
                        Id = request.Employee.Id,
                        FirstName = request.Employee.FirstName,
                        LastName = request.Employee.LastName,
                        Email = request.Employee.Email,
                        Role = employeeRole
                    },
                    LeaveType = new LeaveTypeDto
                    {
                        Id = request.LeaveTypeId,
                        Name = request.LeaveType.Name
                    },
                    DateRequested = request.DateRequested,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Approved = request.Approved
                });
            }

            _logger.LogInformation("Successfully fetched and mapped leave requests");
            return Result.Success(mappedLeaveRequests);
        }
    }
}
