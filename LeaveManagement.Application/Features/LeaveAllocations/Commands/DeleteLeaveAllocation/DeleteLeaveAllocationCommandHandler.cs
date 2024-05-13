using AutoMapper;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, Result<Unit>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            this._leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
            if (leaveAllocation == null)
            {
                return Result.Failure<Unit>(LeaveAllocationErrors.NotFound(request.Id));
            }

            await _leaveAllocationRepository.DeleteAsync(leaveAllocation);
            

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
