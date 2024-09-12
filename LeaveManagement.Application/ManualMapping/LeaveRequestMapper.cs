using Identity.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;

using LeaveManagement.Domain.LeaveRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.ManualMapping
{
    public class LeaveRequestMapper
    {
        public static LeaveRequestListDto MapToLeaveRequestListDto(LeaveRequest leaveRequest)
        {
            return new LeaveRequestListDto
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                DateRequested = leaveRequest.DateRequested,
                
                Approved = leaveRequest.Approved,
                Cancelled = leaveRequest.Cancelled,
                LeaveType = leaveRequest.LeaveType != null ? new LeaveTypeDto
                {
                    Id = leaveRequest.LeaveType.Id,
                    Name = leaveRequest.LeaveType.Name
                } : null,
                Employee = leaveRequest.Employee != null ? new UserDto
                {
                    Id = leaveRequest.Employee.Id,
                    FirstName = leaveRequest.Employee.FirstName,
                    LastName = leaveRequest.Employee.LastName,
                    Email = leaveRequest.Employee.Email,
                    
                } : null
            };
        }

        public static List<LeaveRequestListDto> MapToLeaveRequestListDtos(IEnumerable<LeaveRequest> leaveRequests)
        {
            return leaveRequests.Select(MapToLeaveRequestListDto).ToList();
        }

        
        public static LeaveRequestDetailsDto MapToLeaveRequestDetailsDto(LeaveRequest leaveRequest)
        {
            return new LeaveRequestDetailsDto
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                DateRequested = leaveRequest.DateRequested,
                RequestComments = leaveRequest.RequestComments,
                Approved = leaveRequest.Approved,
                Cancelled = leaveRequest.Cancelled,
                LeaveType = leaveRequest.LeaveType != null ? new LeaveTypeDto
                {
                    Id = leaveRequest.LeaveType.Id,
                    Name = leaveRequest.LeaveType.Name
                } : null,
                Employee = leaveRequest.Employee != null ? new UserDto
                {
                    Id = leaveRequest.Employee.Id,
                    FirstName = leaveRequest.Employee.FirstName,
                    LastName = leaveRequest.Employee.LastName,
                    Email = leaveRequest.Employee.Email,
                    
                } : null
            };
        }
    }
}
