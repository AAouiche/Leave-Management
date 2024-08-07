using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.EmailService;
using LeaveManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Entities.Email;
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
    public class UpdateLeaveRequestCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IAppLogger<UpdateLeaveRequestCommandHandler>> _mockAppLogger;
        private readonly Mock<IValidator<UpdateLeaveRequestCommand>> _mockValidator;
        private readonly UpdateLeaveRequestCommandHandler _handler;

        public UpdateLeaveRequestCommandHandlerTests()
        {
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();
            _mockLeaveTypeRepository = new Mock<ILeaveTypeRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockAppLogger = new Mock<IAppLogger<UpdateLeaveRequestCommandHandler>>();
            _mockValidator = new Mock<IValidator<UpdateLeaveRequestCommand>>();
            _handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepository.Object,
                _mockLeaveTypeRepository.Object,
                _mockMapper.Object,
                _mockEmailSender.Object,
                _mockAppLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateLeaveRequest_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(10),
                LeaveTypeId = 1
            };
            var leaveRequest = new LeaveRequest { Id = command.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), LeaveTypeId = 2 };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync(new LeaveType { Id = command.LeaveTypeId });
            _mockMapper.Setup(m => m.Map(command, leaveRequest)).Verifiable();
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockMapper.Verify(m => m.Map(command, leaveRequest), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(1), 
                LeaveTypeId = 1
            };

            var leaveRequest = new LeaveRequest { Id = command.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), LeaveTypeId = 2 };
            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);

            var validationFailures = new List<ValidationFailure> { new ValidationFailure("StartDate", "Start date must be before end date.") };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(It.IsAny<LeaveRequest>()), Times.Never);
            _mockAppLogger.Verify(logger => logger.LogWarning("Validation failed for command: {Command}", command), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRequestDoesNotExist()
        {
            // Arrange
            var command = new UpdateLeaveRequestCommand { Id = 1 };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The leave request with the Id = '1' was not found", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(It.IsAny<LeaveRequest>()), Times.Never);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailSendingFails()
        {
            // Arrange
            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(10),
                LeaveTypeId = 1
            };
            var leaveRequest = new LeaveRequest { Id = command.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), LeaveTypeId = 2 };

            _mockLeaveRequestRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveRequest);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync(new LeaveType { Id = command.LeaveTypeId });
            _mockMapper.Setup(m => m.Map(command, leaveRequest)).Verifiable();
            _mockLeaveRequestRepository.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);
            _mockEmailSender.Setup(e => e.SendEmail(It.IsAny<Email>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            _mockMapper.Verify(m => m.Map(command, leaveRequest), Times.Once);
            _mockLeaveRequestRepository.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
            _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<Email>()), Times.Once);
            _mockAppLogger.Verify(l => l.LogWarning("Failed to send confirmation email."), Times.Once);
        }
    }
}
