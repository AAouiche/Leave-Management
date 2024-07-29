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

            var mockRepo = new Mock<ILeaveTypeRepository>();

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveTypes);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return leaveTypes.FirstOrDefault(lt => lt.Id == id);
            });

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>())).Returns((LeaveType leaveType) =>
            {
                leaveTypes.Add(leaveType);
                return Task.CompletedTask;
            });

            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveType>())).Returns((LeaveType leaveType) =>
            {
                leaveTypes.Remove(leaveType);
                return Task.CompletedTask;
            });

            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveType>())).Returns((LeaveType leaveType) =>
            {
                var existingLeaveType = leaveTypes.FirstOrDefault(lt => lt.Id == leaveType.Id);
                if (existingLeaveType != null)
                {
                    existingLeaveType.Name = leaveType.Name;
                    existingLeaveType.Days = leaveType.Days;
                }
                return Task.CompletedTask;
            });

            mockRepo.Setup(r => r.CheckExistingName(It.IsAny<string>())).ReturnsAsync((string name) =>
            {
                return leaveTypes.Any(lt => lt.Name == name);
            });

            return mockRepo;
        }
    }
}
