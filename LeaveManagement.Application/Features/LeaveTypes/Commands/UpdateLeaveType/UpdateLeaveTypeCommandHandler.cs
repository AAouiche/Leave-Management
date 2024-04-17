using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Result<int>>
    {
        private readonly IGenericRepository<LeaveType> _repository;

        public UpdateLeaveTypeCommandHandler(IGenericRepository<LeaveType> repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _repository.GetByIdAsync(request.Id);
            if (leaveType == null)
            {
                return Result.Failure<int>(LeaveTypeErrors.NotFound(request.Id));
            }

            leaveType.Name = request.Name;
            leaveType.Days = request.Days;

            await _repository.UpdateAsync(leaveType);
            return Result<int>.Success(leaveType.Id);
        }
    }
}
