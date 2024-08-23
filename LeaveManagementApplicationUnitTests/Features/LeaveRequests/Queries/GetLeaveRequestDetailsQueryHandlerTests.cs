using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries;
using LeaveManagement.Domain.LeaveRequests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveRequests.Queries
{
    public class GetLeaveRequestDetailsQueryHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<GetLeaveRequestDetailsQuery>> _mockValidator;
        private readonly GetLeaveRequestDetailsQueryHandler _handler;

        public GetLeaveRequestDetailsQueryHandlerTests()
        {
            _mockLeaveRequestRepository = new Mock<ILeaveRequestRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockValidator = new Mock<IValidator<GetLeaveRequestDetailsQuery>>();
            _handler = new GetLeaveRequestDetailsQueryHandler(
                _mockLeaveRequestRepository.Object,
                _mockMapper.Object,
                _mockValidator.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnLeaveRequestDetails_WhenQueryIsValid()
        {
            // Arrange
            var query = new GetLeaveRequestDetailsQuery(1);
            var leaveRequest = new LeaveRequest { Id = query.Id };
            var leaveRequestDetailsDto = new LeaveRequestDetailsDto { Id = query.Id };

            _mockValidator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLeaveRequestRepository.Setup(r => r.GetLeaveRequestWithDetails(query.Id)).ReturnsAsync(leaveRequest);
            _mockMapper.Setup(m => m.Map<LeaveRequestDetailsDto>(leaveRequest)).Returns(leaveRequestDetailsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(leaveRequestDetailsDto, result.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var query = new GetLeaveRequestDetailsQuery(0); 
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("LeaveRequestId", "LeaveRequestId must be greater than zero.") };

            _mockValidator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("LeaveRequestId must be greater than zero.", result.Error.Description);
            _mockLeaveRequestRepository.Verify(r => r.GetLeaveRequestWithDetails(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLeaveRequestNotFound()
        {
            // Arrange
            var query = new GetLeaveRequestDetailsQuery(1);

            _mockValidator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLeaveRequestRepository.Setup(r => r.GetLeaveRequestWithDetails(query.Id)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"The leave request with the Id = '{query.Id}' was not found", result.Error.Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMappingFails()
        {
            // Arrange
            var query = new GetLeaveRequestDetailsQuery(1);
            var leaveRequest = new LeaveRequest { Id = query.Id };

            _mockValidator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLeaveRequestRepository.Setup(r => r.GetLeaveRequestWithDetails(query.Id)).ReturnsAsync(leaveRequest);
            _mockMapper.Setup(m => m.Map<LeaveRequestDetailsDto>(leaveRequest)).Returns((LeaveRequestDetailsDto)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Mapping failure", result.Error.Description);
        }
    }
}
