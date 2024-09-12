using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;

namespace LeaveManagement.Domain.LeaveRequests
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<LeaveRequest> GetByIdWithDetailsAsync(int id);
        Task<List<LeaveRequest>> GetAllWithDetailsAsync();
        Task<List<LeaveRequest>> GetAllByUserWithDetailsAsync(string userId);
    }
}
