using LeaveManagement.Shared.Common;
using Identity.Authentication;
using LeaveManagement.Domain.LeaveTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Domain.LeaveRequests
{
    public class LeaveRequest : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public DateTime DateRequested { get; set; }
        public string? RequestComments { get; set; }

        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        public string RequestingEmployeeId { get; set; } = string.Empty;
        public ApplicationUser Employee { get; set; }
        public string? FilePath { get; set; }

    }
}
