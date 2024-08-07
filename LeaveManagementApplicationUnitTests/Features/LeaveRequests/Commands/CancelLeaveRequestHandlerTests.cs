using LeaveManagement.Application.EmailService;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.LeaveRequests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Commands
{
    public class CancelLeaveRequestCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly CancelLeaveRequestCommandHandler _handler;
        private readonly Mock<IAppLogger<CancelLeaveRequestCommandHandler>> _logger;

        public CancelLeaveRequestCommandHandlerTests()
        {
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();
            _mockEmailSender = new Mock<IEmailSender>();
            _logger = new Mock<IAppLogger<CancelLeaveRequestCommandHandler>>();

            _handler = new CancelLeaveRequestCommandHandler(
                _mockLeaveRequestRepository.Object,
                _mockEmailSender.Object,
                _logger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCancelLeaveRequest_WhenCommandIsValid()
        {
            // Arrange
            var command = new CancelLeaveRequestCommand(1);
            var leaveRequest = new LeaveRequest { Id = command.Id, Cancelled = false };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(leaveRequest.Cancelled);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRequestDoesNotExist()
        {
            // Arrange
            var command = new CancelLeaveRequestCommand(1);

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Leave request with ID 1 not found.", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(It.IsAny<LeaveRequest>()), Times.Never);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailSendingFails()
        {
            // Arrange
            var command = new CancelLeaveRequestCommand(1);
            var leaveRequest = new LeaveRequest { Id = command.Id, Cancelled = false };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to send cancellation confirmation email.", result.Error.Description);
            Assert.True(leaveRequest.Cancelled);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }
    }
}
