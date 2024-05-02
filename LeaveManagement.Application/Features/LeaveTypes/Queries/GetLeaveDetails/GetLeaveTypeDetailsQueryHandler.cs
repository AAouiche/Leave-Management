using AutoMapper;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails
{
    public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypesDetailsQuery, Result<LeaveTypeDetailsDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<LeaveTypeDetailsDto>> Handle(GetLeaveTypesDetailsQuery request, CancellationToken cancellationToken)
        {
            var details = await _leaveTypeRepository.GetByIdAsync(request.Id);

            if (details == null)
            {
                return Result.Failure<LeaveTypeDetailsDto>(LeaveTypeErrors.NotFound(request.Id));
            }

            var result = _mapper.Map<LeaveTypeDetailsDto>(details);
            if (result == null)
            {
                
                return Result.Failure<LeaveTypeDetailsDto>(LeaveTypeErrors.MappingError);
            }

            return Result<LeaveTypeDetailsDto>.Success(result);
        }
    }
}
