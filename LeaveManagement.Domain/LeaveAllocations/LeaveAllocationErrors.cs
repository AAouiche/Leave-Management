using FluentValidation.Results;
using LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveAllocation
{
    public static class LeaveAllocationErrors
    {
        public static Error NotFound(int leaveAllocationId) => new(
            "LeaveAllocations.NotFound", $"The leave allocation with the Id = '{leaveAllocationId}' was not found");

        public static Error InsufficientAllocation(int requiredDays, int availableDays) => new(
            "LeaveAllocations.InsufficientAllocation",
            $"Insufficient leave allocation: required {requiredDays}, available {availableDays}");

        public static Error UpdateFailure(int leaveAllocationId) => new(
            "LeaveAllocations.UpdateFailure", $"Failed to update the leave allocation with Id = '{leaveAllocationId}'");

        public static Error DeletionFailure(int leaveAllocationId) => new(
            "LeaveAllocations.DeletionFailure", $"Failed to delete the leave allocation with Id = '{leaveAllocationId}'");
        public static Error NotFoundGeneric() => new(
        "LeaveAllocations.NotFound", "No leave allocations found.");

        public static Error MappingError() => new(
            "LeaveAllocations.MappingError", "Failed to map leave allocations.");
        public static Error ValidationFailure(List<ValidationFailure> failures)
        {
            var errors = string.Join(", ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            return new Error("LeaveAllocations.ValidationFailure", $"Validation failed: {errors}");
        }
    }
}
