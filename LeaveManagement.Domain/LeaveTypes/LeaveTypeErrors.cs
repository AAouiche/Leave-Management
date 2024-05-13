using LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveTypes
{
    public static class LeaveTypeErrors
    {
        public static Error NotFound(int leaveTypeId) => new(
            "LeaveTypes.NotFound", $"The leave type with the Id = '{leaveTypeId}' was not found");

        public static Error NotFoundByName(string name) => new(
            "LeaveTypes.NotFoundByName", $"The leave type with the Name = '{name}' was not found");

        public static readonly Error NameNotUnique = new(
            "LeaveTypes.NameNotUnique", "The provided leave type name is not unique");

        public static Error InvalidDays(int days) => new(
            "LeaveTypes.InvalidDays", $"The number of days '{days}' is not valid for a leave type");
        public static readonly Error MappingError = new(
            "MappingError", "Failed to map leave type details.");

        //new Error("MappingError", "Failed to map leave type details.")
    }
}

