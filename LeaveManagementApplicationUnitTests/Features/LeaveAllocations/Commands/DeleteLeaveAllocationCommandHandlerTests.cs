using AutoMapper;
using LeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.LeaveAllocations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveAllocations.Commands
{
    public class DeleteLeaveAllocationCommandHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAppLogger<DeleteLeaveAllocationCommandHandler>> _mockLogger;
        private readonly DeleteLeaveAllocationCommandHandler _handler;

        public DeleteLeaveAllocationCommandHandlerTests()
        {
            _mockLeaveAllocationRepository = new Mock<ILeaveAllocationRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IAppLogger<DeleteLeaveAllocationCommandHandler>>();
            _handler = new DeleteLeaveAllocationCommandHandler(
                _mockLeaveAllocationRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteLeaveAllocation_WhenExists()
        {
            // Arrange
            var command = new DeleteLeaveAllocationCommand(1);
            var leaveAllocation = new LeaveAllocation { Id = command.Id };

            _mockLeaveAllocationRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveAllocation);
            _mockLeaveAllocationRepository.Setup(r => r.DeleteAsync(leaveAllocation)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockLeaveAllocationRepository.Verify(r => r.DeleteAsync(leaveAllocation), Times.Once);
            _mockLogger.Verify(l => l.LogInformation($"Attempting to delete leave allocation with ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogInformation($"Leave allocation with ID {command.Id} deleted successfully."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveAllocationNotFound()
        {
            // Arrange
            var command = new DeleteLeaveAllocationCommand(1);

            _mockLeaveAllocationRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveAllocation)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The leave allocation with the Id = '1' was not found", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation($"Attempting to delete leave allocation with ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning($"Leave allocation with ID {command.Id} not found."), Times.Once);
        }
    }
}
