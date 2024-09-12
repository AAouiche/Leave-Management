using LeaveManagement.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagementApplicationUnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Commands
{
   /* public class DeleteLeaveRequestCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly DeleteLeaveRequestCommandHandler _handler;

        public DeleteLeaveRequestCommandHandlerTests()
        {
            _mockLeaveRequestRepository = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            _handler = new DeleteLeaveRequestCommandHandler(_mockLeaveRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteLeaveRequest_WhenRequestExists()
        {
            // Arrange
            var command = new DeleteLeaveRequestCommand(1);
            var leaveRequest = new LeaveRequest { Id = command.Id };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(r => r.DeleteAsync(leaveRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockLeaveRequestRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.DeleteAsync(leaveRequest), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRequestDoesNotExist()
        {
            // Arrange
            var command = new DeleteLeaveRequestCommand(1);

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"The leave request with the Id = '{command.Id}' was not found", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.DeleteAsync(It.IsAny<LeaveRequest>()), Times.Never);
        }
    }*/
}
