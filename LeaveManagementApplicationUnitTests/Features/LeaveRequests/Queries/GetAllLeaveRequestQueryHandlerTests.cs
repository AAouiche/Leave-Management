using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Queries
{
    public class GetAllLeaveRequestQueryHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAppLogger<GetAllLeaveRequestQueryHandler>> _mockLogger;
        private readonly GetAllLeaveRequestQueryHandler _handler;

        public GetAllLeaveRequestQueryHandlerTests()
        {
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IAppLogger<GetAllLeaveRequestQueryHandler>>();
            _handler = new GetAllLeaveRequestQueryHandler(
                _mockMapper.Object,
                _mockLeaveRequestRepository.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnLeaveRequests_WhenMappingIsSuccessful()
        {
            // Arrange
                var query = new GetAllLeaveRequestQuery();
                var leaveRequests = new List<LeaveRequest>
                {
                    new LeaveRequest { Id = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), LeaveTypeId = 1, RequestingEmployeeId = "employee1", DateRequested = DateTime.Now, Approved = true },
                    new LeaveRequest { Id = 2, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3), LeaveTypeId = 2, RequestingEmployeeId = "employee2", DateRequested = DateTime.Now.AddDays(1), Approved = false }
                };
                var leaveRequestListDtos = new List<LeaveRequestListDto>
                {
                    new LeaveRequestListDto { RequestingEmployeeId = "employee1", LeaveType = new LeaveTypeDto { Id = 1, Name = "Type1" }, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), DateRequested = DateTime.Now, Approved = true },
                    new LeaveRequestListDto { RequestingEmployeeId = "employee2", LeaveType = new LeaveTypeDto { Id = 2, Name = "Type2" }, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3), DateRequested = DateTime.Now.AddDays(1), Approved = false }
                };

            _mockLeaveRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests);
            _mockMapper.Setup(m => m.Map<List<LeaveRequestListDto>>(leaveRequests)).Returns(leaveRequestListDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(leaveRequestListDtos, result.Value);
            _mockLogger.Verify(l => l.LogInformation("Fetching all leave requests"), Times.Once);
            _mockLogger.Verify(l => l.LogInformation("Successfully fetched and mapped leave requests"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMappingFails()
        {
            // Arrange
            var query = new GetAllLeaveRequestQuery();
            var leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest { Id = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), LeaveTypeId = 1, RequestingEmployeeId = "employee1", DateRequested = DateTime.Now, Approved = true },
                new LeaveRequest { Id = 2, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3), LeaveTypeId = 2, RequestingEmployeeId = "employee2", DateRequested = DateTime.Now.AddDays(1), Approved = false }
            };

            _mockLeaveRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests);
            _mockMapper.Setup(m => m.Map<List<LeaveRequestListDto>>(leaveRequests)).Returns((List<LeaveRequestListDto>)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to map the leave request details.", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation("Fetching all leave requests"), Times.Once);
            _mockLogger.Verify(l => l.LogWarning("Mapping failure occurred while fetching leave requests"), Times.Once);
        }
    }
}
