using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveTypes.Commands
{
    public class DeleteLeaveTypeCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<LeaveType>> _mockRepository;
        private readonly Mock<IAppLogger<DeleteLeaveTypeCommandHandler>> _mockLogger;
        private readonly Mock<IValidator<DeleteLeaveTypeCommand>> _mockValidator;
        private readonly DeleteLeaveTypeCommandHandler _handler;

        public DeleteLeaveTypeCommandHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<LeaveType>>();
            _mockLogger = new Mock<IAppLogger<DeleteLeaveTypeCommandHandler>>();
            _mockValidator = new Mock<IValidator<DeleteLeaveTypeCommand>>();
            _handler = new DeleteLeaveTypeCommandHandler(_mockRepository.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteLeaveType_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteLeaveTypeCommand(1);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", Days = 10 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(leaveType);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
            _mockRepository.Verify(repo => repo.DeleteAsync(leaveType), Times.Once);
            _mockLogger.Verify(logger => logger.LogInformation($"LeaveType with Id {command.Id} deleted successfully."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveTypeDoesNotExist()
        {
            // Arrange
            var command = new DeleteLeaveTypeCommand(1);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((LeaveType)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<LeaveType>()), Times.Never);
            _mockLogger.Verify(logger => logger.LogWarning("Validation failed: LeaveType doesn't exist."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new DeleteLeaveTypeCommand(1);
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Id", "Invalid ID") };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid ID", result.Error.Description);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<LeaveType>()), Times.Never);
            _mockLogger.Verify(logger => logger.LogWarning("Validation failed for command: {Command}", command), Times.Once);
        }
    }
}
