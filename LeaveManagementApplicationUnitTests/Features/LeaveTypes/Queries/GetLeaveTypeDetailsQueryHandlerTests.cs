using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
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
using Shouldly;

namespace LeaveManagementApplicationUnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailsQueryHandlerTests : LeaveTypeBaseTest<GetLeaveTypeDetailsQueryHandler>
    {
        private readonly Mock<IValidator<GetLeaveTypesDetailsQuery>> _mockValidator;
        private readonly GetLeaveTypeDetailsQueryHandler _handler;
        

        public GetLeaveTypeDetailsQueryHandlerTests(Mock<IValidator<GetLeaveTypesDetailsQuery>> mockvalidator, GetLeaveTypeDetailsQueryHandler handler)
            : base()
        {
            _mockValidator = mockvalidator;
            
            _handler = handler;
        }

        [Fact]
        public async Task Handle_ReturnsLeaveTypeDetails_WhenRequestIsValid()
        {
            // Arrange
            var request = new GetLeaveTypesDetailsQuery(1);
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", Days = 10 };
            var leaveTypeDetailsDto = new LeaveTypeDetailsDto { Id = 1, Name = "Vacation", DefaultDays = 10 };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            MockRepo.Setup(repo => repo.GetByIdAsync(request.Id))
                    .ReturnsAsync(leaveType);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<LeaveTypeDetailsDto>(leaveType))
                      .Returns(leaveTypeDetailsDto);

            var handler = new GetLeaveTypeDetailsQueryHandler(mockMapper.Object, MockRepo.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(leaveTypeDetailsDto);
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenValidationFails()
        {
            // Arrange
            var request = new GetLeaveTypesDetailsQuery(-1);
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Id", "Invalid ID")
        };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
       
            
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenLeaveTypeNotFound()
        {
            // Arrange
            var request = new GetLeaveTypesDetailsQuery(99);

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            MockRepo.Setup(repo => repo.GetByIdAsync(request.Id))
                    .ReturnsAsync((LeaveType)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.Code.ShouldBe(LeaveTypeErrors.NotFound(request.Id).Code);
           
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenMappingFails()
        {
            // Arrange
            var request = new GetLeaveTypesDetailsQuery(1);
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", Days = 10 };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            MockRepo.Setup(repo => repo.GetByIdAsync(request.Id))
                    .ReturnsAsync(leaveType);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<LeaveTypeDetailsDto>(leaveType))
                      .Returns((LeaveTypeDetailsDto)null);

            var handler = new GetLeaveTypeDetailsQueryHandler(mockMapper.Object, MockRepo.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.Code.ShouldBe(LeaveTypeErrors.MappingError.Code);
           
        }
    }
}
