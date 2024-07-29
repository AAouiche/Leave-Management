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
    public abstract class LeaveTypeBaseTest<THandler> : BaseTest<ILeaveTypeRepository, THandler>
        where THandler : class
    {
        protected readonly Mock<ILeaveTypeRepository> _MockRepo;
        protected LeaveTypeBaseTest()
        {
            _MockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();
        }
    }
}
