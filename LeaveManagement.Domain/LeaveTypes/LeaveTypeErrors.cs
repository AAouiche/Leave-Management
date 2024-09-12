using LeaveManagement.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveTypes
{
    public static class LeaveTypeErrors
    {
        public static Error NotFound(int leaveTypeId) => Error.NotFound(
            "LeaveTypes.NotFound",
            $"The leave type with the Id = '{leaveTypeId}' was not found");

        public static Error ValidationError(string[] errors) => Error.Validation(
            "LeaveTypes.ValidationErrors",
            string.Join("; ", errors));

        public static Error NotFoundByName(string name) => Error.NotFound(
            "LeaveTypes.NotFoundByName",
            $"The leave type with the Name = '{name}' was not found");

        public static readonly Error NameNotUnique = Error.Conflict(
            "LeaveTypes.NameNotUnique",
            "The provided leave type name is not unique");

        public static Error InvalidDays(int days) => Error.Validation(
            "LeaveTypes.InvalidDays",
            $"The number of days '{days}' is not valid for a leave type");

        public static readonly Error MappingError = Error.Failure(
            "MappingError",
            "Failed to map leave type details.");

        public static readonly Error DataRetrievalError = Error.Failure(
            "DataRetrievalError",
            "Failed to retrieve leave types.");
    }
}

