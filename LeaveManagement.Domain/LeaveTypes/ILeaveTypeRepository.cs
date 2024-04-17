using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Repositories;

namespace LeaveManagement.Domain.LeaveTypes
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<bool> CheckExistingName(String name);
    }
}
