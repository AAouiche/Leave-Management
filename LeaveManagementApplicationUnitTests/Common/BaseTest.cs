using AutoMapper;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Application.Logging;
using LeaveManagement.Application.MappingProfiles;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagementApplicationUnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Common
{
    public abstract class BaseTest<TRepository, THandler>
    where TRepository : class
    where THandler : class
{
    protected readonly Mock<TRepository> MockRepo;
    protected readonly IMapper Mapper;
    protected readonly Mock<IAppLogger<THandler>> MockAppLogger;

    protected BaseTest()
    {
        MockRepo = new Mock<TRepository>();
        Mapper = new MapperConfiguration(cfg => {
            cfg.AddProfile<LeaveTypeProfile>();
        }).CreateMapper();
        MockAppLogger = new Mock<IAppLogger<THandler>>();
    }
}
}
