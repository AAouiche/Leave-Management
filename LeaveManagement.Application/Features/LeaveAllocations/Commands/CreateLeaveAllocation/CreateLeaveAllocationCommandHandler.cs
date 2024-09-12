using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Logging;

using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.Interfaces;
using LeaveManagement.Domain.LeaveAllocation;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Interfaces;

namespace LeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAccessUser _accessUser;

        public CreateLeaveAllocationCommandHandler(IMapper mapper,
            ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository,
            IAccessUser accessUser)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _accessUser = accessUser;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                return Result.Failure<Unit>(LeaveAllocationErrors.ValidationFailure(validationResult.Errors));

            
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

            
            var employees = await _accessUser.GetAllUsers();

            
            var period = DateTime.Now.Year;

            //Assign Allocations IF an allocation doesn't already exist for period and leave type
            var allocations = new List<LeaveAllocation>();
            foreach (var emp in employees)
            {
                var allocationExists = await _leaveAllocationRepository.AllocationExists(emp.Id, request.LeaveTypeId, period);

                if (allocationExists == false)
                {
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = emp.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.Days,
                        Period = period,
                    });
                }
            }

            if (allocations.Any())
            {
                await _leaveAllocationRepository.AddAllocations(allocations);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
