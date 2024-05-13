using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsQuery : IRequest<Result<LeaveAllocationDetailsDto>>
    {
        public GetLeaveAllocationDetailsQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
