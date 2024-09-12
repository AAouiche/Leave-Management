using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveTypes.Commands
{
    public class UpdateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<LeaveType>> _mockRepository;
        private readonly Mock<IAppLogger<UpdateLeaveTypeCommandHandler>> _mockLogger;
        private readonly Mock<IValidator<UpdateLeaveTypeCommand>> _mockValidator;
        private readonly UpdateLeaveTypeCommandHandler _handler;

        public UpdateLeaveTypeCommandHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<LeaveType>>();
            _mockLogger = new Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>();
            _mockValidator = new Mock<IValidator<UpdateLeaveTypeCommand>>();
            _handler = new UpdateLeaveTypeCommandHandler(_mockRepository.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateLeaveType_WhenCommandIsValid()
        {
            // Arrange
            var leaveType = new LeaveType { Id = 1, Name = "Updated Vacation", Days = 15 };
            var command = new UpdateLeaveTypeCommand(leaveType);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(new LeaveType { Id = 1, Name = "Vacation", Days = 10 });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<LeaveType>(lt => lt.Id == 1 && lt.Name == "Updated Vacation" && lt.Days == 15)), Times.Once);
            _mockLogger.Verify(logger => logger.LogInformation($"LeaveType with Id {command.Id} updated successfully."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveTypeDoesNotExist()
        {
            // Arrange
            var leaveType = new LeaveType { Id = 1, Name = "Updated Vacation", Days = 15 };
            var command = new UpdateLeaveTypeCommand(leaveType);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((LeaveType)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The leave type with the Id = '1' was not found", result.Error.Description);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<LeaveType>()), Times.Never);
            _mockLogger.Verify(logger => logger.LogWarning($"LeaveType with Id {command.Id} not found."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var leaveType = new LeaveType { Id = 1, Name = "Updated Vacation", Days = 15 };
            var command = new UpdateLeaveTypeCommand(leaveType);
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Name is required", result.Error.Description);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<LeaveType>()), Times.Never);
            _mockLogger.Verify(logger => logger.LogWarning("Validation failed for command: {Command}", command), Times.Once);
        }
    }
}
