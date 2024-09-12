using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;

namespace LeaveManagement.Domain.LeaveTypes
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<bool> CheckExistingName(String name);
    }
}
