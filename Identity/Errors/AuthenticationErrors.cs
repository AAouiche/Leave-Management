using FluentValidation.Results;
using LeaveManagement.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Errors
{
    public static class AuthenticationErrors
    {
        public static Error InvalidCredentials() => Error.Unauthorized(
            "Authentication.InvalidCredentials",
            "The provided credentials are invalid.");

        public static Error UserNotFound(string email) => Error.NotFound(
            "Authentication.UserNotFound",
            $"The user with the email '{email}' does not exist.");

        public static Error AccountLocked(string userName) => Error.Failure(
            "Authentication.AccountLocked",
            $"The account for user '{userName}' is locked due to multiple failed login attempts.");

        public static Error PasswordMismatch() => Error.Validation(
            "Authentication.PasswordMismatch",
            "The password provided does not match our records.");

        public static Error UnauthorizedAccess() => Error.Unauthorized(
            "Authentication.UnauthorizedAccess",
            "You do not have permission to perform this action.");

        public static Error ValidationFailure(List<ValidationFailure> failures)
        {
            var errors = string.Join(", ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            return Error.Validation("Authentication.ValidationFailure", $"Validation failed: {errors}");
        }
        // Registration-specific errors
        public static Error EmailAlreadyInUse(string email) => Error.Conflict(
            "Authentication.EmailAlreadyInUse",
            $"The email '{email}' is already in use. Please use a different email address.");

        public static Error WeakPassword() => Error.Validation(
            "Authentication.WeakPassword",
            "The password does not meet the security requirements.");

        public static Error UserCreationFailed(IEnumerable<string> errors) => Error.Failure(
            "Authentication.UserCreationFailed",
            $"User creation failed: {string.Join(", ", errors)}");
    }
}
