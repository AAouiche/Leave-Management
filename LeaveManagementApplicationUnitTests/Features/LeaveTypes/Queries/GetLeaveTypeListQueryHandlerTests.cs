using AutoMapper;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Application.Logging;
using LeaveManagement.Application.MappingProfiles;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagementApplicationUnitTests.Common;
using LeaveManagementApplicationUnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeListQueryHandlerTests : LeaveTypeBaseTest<GetLeaveTypesQueryHandler>
    {
        private readonly IMapper _mapper;
        private readonly GetLeaveTypesQueryHandler _handler;
        private readonly Mock<IAppLogger<GetLeaveTypesQueryHandler>> _mockAppLogger;


        public GetLeaveTypeListQueryHandlerTests()
            : base()
        {
            _mockAppLogger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();
            _handler = new GetLeaveTypesQueryHandler(Mapper, MockRepo.Object, _mockAppLogger.Object);
        }

        [Fact]
        public async Task Handle_ReturnsLeaveTypeList_WhenLeaveTypesExist()
        {
            // Act
            var result = await _handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Value.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenNoLeaveTypesExist()
        {
            // Arrange
            MockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync((List<LeaveType>)null);

            // Act
            var result = await _handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.Code.ShouldBe(LeaveTypeErrors.DataRetrievalError.Code);
            
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenMappingFails()
        {
            // Arrange
            var leaveTypes = new List<LeaveType>
        {
            new LeaveType { Id = 1, Name = "Vacation", Days = 10 }
        };

            MockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(leaveTypes);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<List<LeaveTypeDto>>(It.IsAny<List<LeaveType>>())).Returns((List<LeaveTypeDto>)null);

            var handler = new GetLeaveTypesQueryHandler(mockMapper.Object, MockRepo.Object, _mockAppLogger.Object);

            // Act
            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.Code.ShouldBe(LeaveTypeErrors.MappingError.Code);
            
        }
    }

}
