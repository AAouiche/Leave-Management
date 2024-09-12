using FluentValidation.Results;
using LeaveManagement.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveAllocation
{
    public static class LeaveAllocationErrors
    {
        public static Error NotFound(int leaveAllocationId) => Error.NotFound(
            "LeaveAllocations.NotFound",
            $"The leave allocation with the Id = '{leaveAllocationId}' was not found");

        public static Error InsufficientAllocation(int requiredDays, int availableDays) => Error.Failure(
            "LeaveAllocations.InsufficientAllocation",
            $"Insufficient leave allocation: required {requiredDays}, available {availableDays}");

        public static Error UpdateFailure(int leaveAllocationId) => Error.Failure(
            "LeaveAllocations.UpdateFailure",
            $"Failed to update the leave allocation with Id = '{leaveAllocationId}'");

        public static Error DeletionFailure(int leaveAllocationId) => Error.Failure(
            "LeaveAllocations.DeletionFailure",
            $"Failed to delete the leave allocation with Id = '{leaveAllocationId}'");

        public static Error NotFoundGeneric() => Error.NotFound(
            "LeaveAllocations.NotFound",
            "No leave allocations found.");

        public static Error MappingError() => Error.Failure(
            "LeaveAllocations.MappingError",
            "Failed to map leave allocations.");

        public static Error ValidationFailure(List<ValidationFailure> failures)
        {
            var errors = string.Join(", ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            return Error.Validation("LeaveAllocations.ValidationFailure", $"Validation failed: {errors}");
        }
    }
}
