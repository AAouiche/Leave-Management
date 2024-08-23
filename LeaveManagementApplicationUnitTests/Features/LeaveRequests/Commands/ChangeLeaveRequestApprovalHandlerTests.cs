using AutoMapper;
using LeaveManagement.Domain.EmailService;
using LeaveManagement.Application.Features.LeaveRequests.Commands.ChangeLeaveRequestApproval;
using LeaveManagement.Domain.EmailMessage;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Commands
{
    public class ChangeLeaveRequestApprovalCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly ChangeLeaveRequestApprovalCommandHandler _handler;

        public ChangeLeaveRequestApprovalCommandHandlerTests()
        {
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailSender = new Mock<IEmailSender>();
            _handler = new ChangeLeaveRequestApprovalCommandHandler(
                _mockLeaveRequestRepository.Object,
                null, 
                _mockMapper.Object,
                _mockEmailSender.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldChangeLeaveRequestApproval_WhenCommandIsValid()
        {
            // Arrange
            var command = new ChangeLeaveRequestApprovalCommand { Id = 1, Approved = true };
            var leaveRequest = new LeaveRequest { Id = command.Id, Approved = false };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.Approved, leaveRequest.Approved);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRequestDoesNotExist()
        {
            // Arrange
            var command = new ChangeLeaveRequestApprovalCommand { Id = 1, Approved = true };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"The leave request with the Id = '{command.Id}' was not found", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(It.IsAny<LeaveRequest>()), Times.Never);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailSendingFails()
        {
            // Arrange
            var command = new ChangeLeaveRequestApprovalCommand { Id = 1, Approved = true };
            var leaveRequest = new LeaveRequest { Id = command.Id, Approved = false };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to send confirmation email.", result.Error.Description);
            Assert.Equal(command.Approved, leaveRequest.Approved);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }
    }
}
