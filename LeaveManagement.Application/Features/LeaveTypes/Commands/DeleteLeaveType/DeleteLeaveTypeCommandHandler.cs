using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Result<int>>
    {
        private readonly IGenericRepository<LeaveType> _repository;

        public DeleteLeaveTypeCommandHandler(IGenericRepository<Domain.LeaveTypes.LeaveType> repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _repository.GetByIdAsync(request.Id);
            if (leaveType == null)
            {
                
                return Result.Failure<int>(LeaveTypeErrors.NotFound(request.Id));
            }

            await _repository.DeleteAsync(leaveType);
            return Result<int>.Success(request.Id);
        }
    }
}
