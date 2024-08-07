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
        public static Error NotFound(int leaveRequestId) => Error.NotFound(
            "LeaveRequests.NotFound",
            $"The leave request with the Id = '{leaveRequestId}' was not found");

        public static Error ValidationFailure(string details) => Error.Validation(
            "LeaveRequests.ValidationFailure",
            $"Validation failed: {details}");

        public static Error UpdateFailure(int leaveRequestId) => Error.Failure(
            "LeaveRequests.UpdateFailure",
            $"Failed to update the leave request with Id = '{leaveRequestId}'");

        public static Error EmailFailure(string email) => Error.Failure(
            "LeaveRequests.EmailFailure",
            $"Failed to send confirmation email to '{email}'");

        public static readonly Error UnauthorizedAccess = Error.Failure(
            "LeaveRequests.UnauthorizedAccess",
            "Unauthorized access to the leave request");

        public static Error MappingFailure() => Error.Failure(
            "LeaveRequests.MappingFailure",
            "Failed to map the leave request details.");
    }
}
