using LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveRequests
{
    public static class LeaveRequestErrors
    {
        public static Error NotFound(int leaveRequestId) => new(
            "LeaveRequests.NotFound", $"The leave request with the Id = '{leaveRequestId}' was not found");

        public static Error ValidationFailure(string details) => new(
            "LeaveRequests.ValidationFailure", $"Validation failed: {details}");

        public static Error UpdateFailure(int leaveRequestId) => new(
            "LeaveRequests.UpdateFailure", $"Failed to update the leave request with Id = '{leaveRequestId}'");

        public static Error EmailFailure(string email) => new(
            "LeaveRequests.EmailFailure", $"Failed to send confirmation email.");
       

        public static readonly Error UnauthorizedAccess = new(
            "LeaveRequests.UnauthorizedAccess", "Unauthorized access to the leave request");
        public static Error MappingFailure() => new(
            "LeaveRequests.MappingFailure", "Failed to map the leave request details.");
    }
}
