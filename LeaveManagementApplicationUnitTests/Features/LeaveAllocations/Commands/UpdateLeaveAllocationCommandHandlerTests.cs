using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveAllocations.Commands
{
    public class UpdateLeaveAllocationCommandHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepository;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAppLogger<UpdateLeaveAllocationCommandHandler>> _mockLogger;
        private readonly UpdateLeaveAllocationCommandHandler _handler;
        private readonly Mock<IValidator<UpdateLeaveAllocationCommand>> _mockValidator;

        public UpdateLeaveAllocationCommandHandlerTests()
        {
            _mockLeaveAllocationRepository = new Mock<ILeaveAllocationRepository>();
            _mockLeaveTypeRepository = new Mock<ILeaveTypeRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IAppLogger<UpdateLeaveAllocationCommandHandler>>();
            _mockValidator = new Mock<IValidator<UpdateLeaveAllocationCommand>>();
            _handler = new UpdateLeaveAllocationCommandHandler(
                _mockMapper.Object,
                _mockLeaveTypeRepository.Object,
                _mockLeaveAllocationRepository.Object,
                _mockLogger.Object
            );
        }

        private UpdateLeaveAllocationCommand CreateCommand(int id, int leaveTypeId, int numberOfDays, int period)
        {
            return new UpdateLeaveAllocationCommand
            {
                Id = id,
                LeaveTypeId = leaveTypeId,
                NumberOfDays = numberOfDays,
                Period = period
            };
        }

        [Fact]
        public async Task Handle_ShouldUpdateLeaveAllocation_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateCommand(1, 1, 10, 2024);
            var leaveAllocation = new LeaveAllocation { Id = command.Id };

            _mockLeaveAllocationRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveAllocation);
            _mockLeaveAllocationRepository.Setup(r => r.UpdateAsync(leaveAllocation)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map(command, leaveAllocation)).Returns(leaveAllocation);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockLeaveAllocationRepository.Verify(r => r.UpdateAsync(leaveAllocation), Times.Once);
            _mockLogger.Verify(l => l.LogInformation($"Validating the update leave allocation request for ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogInformation($"Leave allocation with ID {command.Id} updated successfully."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = CreateCommand(1, 1, 0, 2024); 
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("NumberOfDays", "Number of days must be greater than zero.") };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Validation failed", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation($"Validating the update leave allocation request for ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning($"Validation failed for update leave allocation request for ID {command.Id}."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveAllocationNotFound()
        {
            // Arrange
            var command = CreateCommand(1, 1, 10, 2024);
            _mockLeaveAllocationRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((LeaveAllocation)null);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The leave allocation with the Id = '1' was not found", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation($"Validating the update leave allocation request for ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning($"Leave allocation with ID {command.Id} not found."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveTypeNotFound()
        {
            // Arrange
            var command = CreateCommand(1, 1, 10, 2024);
            var leaveAllocation = new LeaveAllocation { Id = command.Id };

            _mockLeaveAllocationRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(leaveAllocation);
            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync((LeaveType)null);
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("LeaveTypeId does not exist.", result.Error.Description);
            _mockLogger.Verify(l => l.LogInformation($"Validating the update leave allocation request for ID {command.Id}."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning($"Leave type with ID {command.LeaveTypeId} not found."), Times.Once);
        }
    }
}

