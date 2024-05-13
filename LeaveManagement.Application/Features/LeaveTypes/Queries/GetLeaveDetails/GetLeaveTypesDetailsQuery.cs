using LeaveManagement.Application.Features.LeaveTypes.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails
{
    public class GetLeaveTypesDetailsQuery : IRequest<Result<LeaveTypeDetailsDto>>
    {
        public GetLeaveTypesDetailsQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
