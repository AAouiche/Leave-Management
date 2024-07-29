using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Dtos
{
    public class CreateLeaveTypeDto
    {
        public string Name { get; set; }
        public int Days { get; set; }
    }
}
