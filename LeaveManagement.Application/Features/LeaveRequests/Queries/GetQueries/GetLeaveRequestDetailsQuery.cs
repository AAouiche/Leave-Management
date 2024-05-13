using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries
{
    public class GetLeaveRequestDetailsQuery
    {
        public GetLeaveRequestDetailsQuery(int id)
        {
            LeaveRequestId = id;
        }
        public int LeaveRequestId { get; set; }

        
    }
}
