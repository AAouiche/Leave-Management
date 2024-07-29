using FluentValidation;
using FluentValidation.Results;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagementApplicationUnitTests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests : LeaveTypeBaseTest<CreateLeaveTypeCommandHandler>
    {
        private readonly Mock<IValidator<CreateLeaveTypeCommand>> _mockValidator;
        private readonly CreateLeaveTypeCommandHandler _handler;

        public CreateLeaveTypeCommandHandlerTests()
            : base()
        {
            _mockValidator = new Mock<IValidator<CreateLeaveTypeCommand>>();
            _handler = new CreateLeaveTypeCommandHandler(_MockRepo.Object, MockAppLogger.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateLeaveType_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateLeaveTypeCommand(new CreateLeaveTypeDto { Name = "Vacation", Days = 10 });
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _MockRepo.Setup(repo => repo.CheckExistingName(command.Name))
                     .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _MockRepo.Verify(repo => repo.CreateAsync(It.IsAny<LeaveType>()), Times.Once);
            MockAppLogger.Verify(logger => logger.LogInformation("LeaveType '{Name}' created successfully.", command.Name), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new CreateLeaveTypeCommand(new CreateLeaveTypeDto { Name = "Vacation", Days = 10 });
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            
            _MockRepo.Verify(repo => repo.CreateAsync(It.IsAny<LeaveType>()), Times.Never);
            MockAppLogger.Verify(logger => logger.LogWarning("Validation failed for command: {Command}", command), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNameIsNotUnique()
        {
            // Arrange
            var command = new CreateLeaveTypeCommand(new CreateLeaveTypeDto { Name = "Vacation", Days = 10 });
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _MockRepo.Setup(repo => repo.CheckExistingName(command.Name))
                     .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            
            _MockRepo.Verify(repo => repo.CreateAsync(It.IsAny<LeaveType>()), Times.Never);
            MockAppLogger.Verify(logger => logger.LogWarning("Validation failed: Name exists."), Times.Once);
        }
    }
}
