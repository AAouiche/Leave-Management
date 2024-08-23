using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Common
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        
        public static Error NotFound(string code, string description) => new Error(code, description, ErrorType.NotFound);

        public static Error Validation(string code, string description) => new Error(code, description, ErrorType.Validation);

        public static Error Conflict(string code, string description) => new Error(code, description, ErrorType.Conflict);

        public static Error Failure(string code, string description) => new Error(code, description, ErrorType.Failure);

        public static Error Unauthorized(string code, string description) => new Error(code, description, ErrorType.Unauthorized);
    }

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4, 
    }
}
