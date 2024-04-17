using LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.LeaveTypes
{
    public class LeaveType : BaseEntity
    {

        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}
