using LeaveManagement.Domain.LeaveRequests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Mocks
{
    public static class MockLeaveRequestRepository
    {
        public static Mock<ILeaveRequestRepository> GetMockLeaveRequestRepository()
        {
            var leaveRequests = new List<LeaveRequest>
        {
            new LeaveRequest
            {
                Id = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
                RequestingEmployeeId = "user1"
            },
            new LeaveRequest
            {
                Id = 2,
                LeaveTypeId = 2,
                StartDate = DateTime.Now.AddDays(-20),
                EndDate = DateTime.Now.AddDays(-15),
                RequestingEmployeeId = "user2"
            }
        };

            var mockRepo = new Mock<ILeaveRequestRepository>();

            // Mock the generic methods
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return leaveRequests.FirstOrDefault(lr => lr.Id == id);
            });
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveRequest>())).Returns((LeaveRequest leaveRequest) =>
            {
                leaveRequests.Add(leaveRequest);
                return Task.CompletedTask;
            });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveRequest>())).Returns((LeaveRequest leaveRequest) =>
            {
                leaveRequests.Remove(leaveRequest);
                return Task.CompletedTask;
            });
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveRequest>())).Returns((LeaveRequest leaveRequest) =>
            {
                var existingLeaveRequest = leaveRequests.FirstOrDefault(lr => lr.Id == leaveRequest.Id);
                if (existingLeaveRequest != null)
                {
                    existingLeaveRequest.LeaveTypeId = leaveRequest.LeaveTypeId;
                    existingLeaveRequest.StartDate = leaveRequest.StartDate;
                    existingLeaveRequest.EndDate = leaveRequest.EndDate;
                    existingLeaveRequest.RequestingEmployeeId = leaveRequest.RequestingEmployeeId;
                }
                return Task.CompletedTask;
            });

            // Mock specific methods
            mockRepo.Setup(r => r.GetLeaveRequestsWithDetails()).ReturnsAsync(leaveRequests);
            mockRepo.Setup(r => r.GetLeaveRequestsWithDetails(It.IsAny<string>())).ReturnsAsync((string userId) =>
            {
                return leaveRequests.Where(lr => lr.RequestingEmployeeId == userId).ToList();
            });
            mockRepo.Setup(r => r.GetLeaveRequestWithDetails(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return leaveRequests.FirstOrDefault(lr => lr.Id == id);
            });

            return mockRepo;
        }
    }
}
