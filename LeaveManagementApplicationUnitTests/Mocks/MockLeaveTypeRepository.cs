using LeaveManagement.Domain.LeaveTypes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementApplicationUnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id = 1,
                    Days = 10,
                    Name = "Vacation"
                },
                new LeaveType
                {
                    Id = 2,
                    Days = 15,
                    Name = "Maternity"
                },
                new LeaveType
                {
                    Id = 3,
                    Days = 15,
                    Name = "Sick"
                }
            };
        }
        }
}
