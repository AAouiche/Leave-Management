using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;
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
    /*public class CreateLeaveAllocationCommandHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepository;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAppLogger<CreateLeaveAllocationCommandHandler>> _mockLogger;
        private readonly CreateLeaveAllocationCommandHandler _handler;

        public CreateLeaveAllocationCommandHandlerTests()
        {
            _mockLeaveAllocationRepository = new Mock<ILeaveAllocationRepository>();
            _mockLeaveTypeRepository = new Mock<ILeaveTypeRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IAppLogger<CreateLeaveAllocationCommandHandler>>();
            _handler = new CreateLeaveAllocationCommandHandler(
                _mockMapper.Object,
                _mockLeaveAllocationRepository.Object,
                _mockLeaveTypeRepository.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateLeaveAllocation_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateLeaveAllocationCommand
            {
                LeaveTypeId = 1
            };

            var leaveType = new LeaveType { Id = 1, Name = "Vacation" };
            var leaveAllocation = new LeaveAllocation { Id = 1, LeaveTypeId = 1, NumberOfDays = 10, EmployeeId = "employee1" };

            _mockLeaveTypeRepository.Setup(r => r.GetByIdAsync(command.LeaveTypeId)).ReturnsAsync(leaveType);
            _mockMapper.Setup(m => m.Map<LeaveAllocation>(command)).Returns(leaveAllocation);
            _mockLeaveAllocationRepository.Setup(r => r.CreateAsync(leaveAllocation)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockLeaveAllocationRepository.Verify(r => r.CreateAsync(leaveAllocation), Times.Once);
            _mockLogger.Verify(l => l.LogInformation("Validating the create leave allocation request."), Times.Once);
            _mockLogger.Verify(l => l.LogInformation($"Leave allocation created successfully for leave type ID {command.LeaveTypeId}."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new CreateLeaveAllocationCommand
            {
                LeaveTypeId = 1
            };

            var validationFailures = new List<ValidationFailure> { new ValidationFailure("LeaveTypeId", "LeaveTypeId must be greater than zero.") };
            var validator = new Mock<IValidator<CreateLeaveAllocationCommand>>();
            validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            var handler = new CreateLeaveAllocationCommandHandler(_mockMapper.Object, _mockLeaveAllocationRepository.Object, _mockLeaveTypeRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            _mockLogger.Verify(l => l.LogInformation("Validating the create leave allocation request."), Times.Once);
            _mockLogger.Verify(l => l.LogWarning("Validation failed for create leave allocation request."), Times.Once);
        }

        
    }*/
}
