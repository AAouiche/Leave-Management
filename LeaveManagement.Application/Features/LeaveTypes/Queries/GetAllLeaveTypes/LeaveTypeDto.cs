using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes
{
    public class LeaveTypeDto
    {
        public int Id { get; set; }
        public string Name {  get; set; } = string.Empty;   
        public int Days { get; set; }
    }
}
