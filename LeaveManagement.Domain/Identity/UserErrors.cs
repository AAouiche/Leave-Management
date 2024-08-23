using LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Identity
{
    public static class UserErrors
    {
        public static Error HttpContextNull() => Error.Failure(
            "GetUser.HttpContextNull",
            "HttpContext or User is null.");

        public static Error UserIdNotFoundInClaims() => Error.Unauthorized(
            "GetUser.UserIdNotFoundInClaims",
            "User ID not found in claims.");

        public static Error UserNotFound(string userId) => Error.NotFound(
            "GetUser.UserNotFound",
            $"User not found in the database for ID: '{userId}'.");

        public static Error NoUsersFound() => Error.NotFound(
            "GetUser.NoUsersFound",
            "No users found in the database.");

        public static Error UnauthorizedAccess() => Error.Failure(
            "GetUser.UnauthorizedAccess",
            "You do not have permission to perform this action.");
    }
}
