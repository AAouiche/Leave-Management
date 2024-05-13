using AutoMapper;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public CreateLeaveAllocationCommandHandler(IMapper mapper,
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                return Result.Failure<Unit>(LeaveAllocationErrors.ValidationFailure(validationResult.Errors));
            }

            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);
            if (leaveType == null)
            {
                return Result.Failure<Unit>(LeaveAllocationErrors.NotFound(request.LeaveTypeId));
            }

            var leaveAllocation = _mapper.Map<LeaveAllocation>(request);
            await _leaveAllocationRepository.CreateAsync(leaveAllocation);
           

            // Assuming success of the creation process, return success.
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
