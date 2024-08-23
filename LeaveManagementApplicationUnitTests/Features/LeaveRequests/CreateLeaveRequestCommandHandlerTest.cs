using AutoMapper;
using LeaveManagement.Domain.EmailService;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using LeaveManagement.Domain.EmailMessage;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagementApplicationUnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Commands
{
    public class CreateLeaveRequestCommandHandlerTests
    {
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly CreateLeaveRequestCommandHandler _handler;

        public CreateLeaveRequestCommandHandlerTests()
        {
            _mockEmailSender = new Mock<IEmailSender>();
            _mockMapper = new Mock<IMapper>();
            _mockLeaveTypeRepository = new Mock<ILeaveTypeRepository>();
            _mockLeaveRequestRepository = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            _handler = new CreateLeaveRequestCommandHandler(
                _mockEmailSender.Object,
                _mockMapper.Object,
                _mockLeaveTypeRepository.Object,
                _mockLeaveRequestRepository.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateLeaveRequest_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateLeaveRequestCommand
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(10),
                LeaveTypeId = 1
            };
            var leaveRequest = new LeaveRequest { Id = 3, LeaveTypeId = command.LeaveTypeId, StartDate = command.StartDate, EndDate = command.EndDate };
            var email = new Email
            {
                Reciever = string.Empty,
                MessageBody = $"Your leave request for {command.StartDate:D} to {command.EndDate:D} has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            _mockMapper.Setup(m => m.Map<LeaveRequest>(command)).Returns(leaveRequest);
            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync(new LeaveType { Id = command.LeaveTypeId });
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockLeaveRequestRepository.Verify(r => r.CreateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new CreateLeaveRequestCommand
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(10),
                LeaveTypeId = 1
            };
            var validator = new CreateLeaveRequestCommandValidator(_mockLeaveTypeRepository.Object);
            var validationResult = await validator.ValidateAsync(command);
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid Leave Request", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.CreateAsync(It.IsAny<LeaveRequest>()), Times.Never);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailSendingFails()
        {
            // Arrange
            var command = new CreateLeaveRequestCommand
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(10),
                LeaveTypeId = 1
            };
            var leaveRequest = new LeaveRequest { Id = 3, LeaveTypeId = command.LeaveTypeId, StartDate = command.StartDate, EndDate = command.EndDate };
            var email = new Email
            {
                Reciever = string.Empty,
                MessageBody = $"Your leave request for {command.StartDate:D} to {command.EndDate:D} has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            _mockMapper.Setup(m => m.Map<LeaveRequest>(command)).Returns(leaveRequest);
            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync(new LeaveType { Id = command.LeaveTypeId });
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to send confirmation email.", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.CreateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }
    }
}
