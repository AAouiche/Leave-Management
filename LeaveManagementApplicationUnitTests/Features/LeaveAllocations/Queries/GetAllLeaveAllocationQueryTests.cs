using AutoMapper;
using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllAllocations;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.LeaveAllocations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveAllocations.Queries
{
    public class GetAllLeaveAllocationsQueryHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAppLogger<GetAllLeaveAllocationsQueryHandler>> _mockLogger;
        private readonly GetAllLeaveAllocationsQueryHandler _handler;

        public GetAllLeaveAllocationsQueryHandlerTests()
        {
            _mockLeaveAllocationRepository = new Mock<ILeaveAllocationRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IAppLogger<GetAllLeaveAllocationsQueryHandler>>();
            _handler = new GetAllLeaveAllocationsQueryHandler(
                _mockLeaveAllocationRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnLeaveAllocations_WhenTheyExist()
        {
            // Arrange
            var leaveAllocations = new List<LeaveAllocation>
            {
                new LeaveAllocation { Id = 1 },
                new LeaveAllocation { Id = 2 }
            };
            var leaveAllocationDtos = new List<LeaveAllocationDto>
            {
                new LeaveAllocationDto { Id = 1 },
                new LeaveAllocationDto { Id = 2 }
            };

            _mockLeaveAllocationRepository.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(leaveAllocations);
            _mockMapper.Setup(m => m.Map<List<LeaveAllocationDto>>(leaveAllocations)).Returns(leaveAllocationDtos);

            // Act
            var result = await _handler.Handle(new GetAllLeaveAllocationQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(leaveAllocationDtos.Count, result.Value.Count);
            _mockLogger.Verify(l => l.LogInformation("Retrieving all leave allocations with details."), Times.Once);
            _mockLogger.Verify(l => l.LogInformation("Successfully retrieved and mapped leave allocations."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoLeaveAllocationsFound()
        {
            // Arrange
            _mockLeaveAllocationRepository.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(new List<LeaveAllocation>());

            // Act
            var result = await _handler.Handle(new GetAllLeaveAllocationQuery(), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No leave allocations found.", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation("Retrieving all leave allocations with details."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning("No leave allocations found."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMappingFails()
        {
            // Arrange
            var leaveAllocations = new List<LeaveAllocation>
            {
                new LeaveAllocation { Id = 1 },
                new LeaveAllocation { Id = 2 }
            };

            _mockLeaveAllocationRepository.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(leaveAllocations);
            _mockMapper.Setup(m => m.Map<List<LeaveAllocationDto>>(leaveAllocations)).Returns((List<LeaveAllocationDto>)null);

            // Act
            var result = await _handler.Handle(new GetAllLeaveAllocationQuery(), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to map leave allocations.", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation("Retrieving all leave allocations with details."), Times.Once);
            _mockLogger.Verify(l => l.LogError("Failed to map leave allocations."), Times.Once);
        }
    }
}
