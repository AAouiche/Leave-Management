using LeaveManagement.Domain.LeaveAllocations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Mocks
{
    public static class MockLeaveAllocationRepository
    {
        public static Mock<ILeaveAllocationRepository> GetMockLeaveAllocationRepository()
        {
            var leaveAllocations = new List<LeaveAllocation>
        {
            new LeaveAllocation
            {
                Id = 1,
                LeaveTypeId = 1,
                NumberOfDays = 10,
                Period = 2021,
                EmployeeId = "1" 
            },
            new LeaveAllocation
            {
                Id = 2,
                LeaveTypeId = 2,
                NumberOfDays = 15,
                Period = 2021,
                EmployeeId = "1" 
            }
        };

            var mockRepo = new Mock<ILeaveAllocationRepository>();

            // Mock the generic methods
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveAllocations);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return leaveAllocations.FirstOrDefault(la => la.Id == id);
            });
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveAllocation>())).Returns((LeaveAllocation leaveAllocation) =>
            {
                leaveAllocations.Add(leaveAllocation);
                return Task.CompletedTask;
            });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveAllocation>())).Returns((LeaveAllocation leaveAllocation) =>
            {
                leaveAllocations.Remove(leaveAllocation);
                return Task.CompletedTask;
            });
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveAllocation>())).Returns((LeaveAllocation leaveAllocation) =>
            {
                var existingLeaveAllocation = leaveAllocations.FirstOrDefault(la => la.Id == leaveAllocation.Id);
                if (existingLeaveAllocation != null)
                {
                    existingLeaveAllocation.LeaveTypeId = leaveAllocation.LeaveTypeId;
                    existingLeaveAllocation.NumberOfDays = leaveAllocation.NumberOfDays;
                    existingLeaveAllocation.Period = leaveAllocation.Period;
                    existingLeaveAllocation.EmployeeId = leaveAllocation.EmployeeId;
                }
                return Task.CompletedTask;
            });

            // Mock specific methods
            mockRepo.Setup(r => r.AddAllocations(It.IsAny<List<LeaveAllocation>>())).Returns((List<LeaveAllocation> allocations) =>
            {
                leaveAllocations.AddRange(allocations);
                return Task.CompletedTask;
            });
            mockRepo.Setup(r => r.AllocationExists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((string userId, int leaveTypeId, int period) =>
            {
                return leaveAllocations.Any(la => la.EmployeeId == userId && la.LeaveTypeId == leaveTypeId && la.Period == period);
            });
            mockRepo.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(leaveAllocations);
            mockRepo.Setup(r => r.GetLeaveAllocationsWithDetails(It.IsAny<int>())).ReturnsAsync((string userId) =>
            {
                return leaveAllocations.Where(la => la.EmployeeId == userId).ToList();
            });
            mockRepo.Setup(r => r.GetLeaveAllocationWithDetails(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return leaveAllocations.FirstOrDefault(la => la.Id == id);
            });
            mockRepo.Setup(r => r.GetUserAllocations(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((string userId, int leaveTypeId) =>
            {
                return leaveAllocations.FirstOrDefault(la => la.EmployeeId == userId && la.LeaveTypeId == leaveTypeId);
            });

            return mockRepo;
        }
    }
}
