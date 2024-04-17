using AutoMapper;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes
{
    public class GetLeavesTypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, Result<List<LeaveTypeDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public GetLeavesTypesQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Result<List<LeaveTypeDto>>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _leaveTypeRepository.GetAllAsync();
            if (leaveTypes == null)
            {
                
                return Result.Failure<List<LeaveTypeDto>>(new Error("DataRetrievalError", "Failed to retrieve leave types."));
            }

            var returnData = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            if (returnData == null)
            {
                
                return Result.Failure<List<LeaveTypeDto>>(new Error("MappingError", "Failed to map leave types."));
            }

            return Result<List<LeaveTypeDto>>.Success(returnData);
        }
    }
}
