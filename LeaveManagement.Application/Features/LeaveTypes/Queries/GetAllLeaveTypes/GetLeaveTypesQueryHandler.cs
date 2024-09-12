using AutoMapper;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes
{
    public class GetLeaveTypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, Result<List<LeaveTypeDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<GetLeaveTypesQueryHandler> _logger;

        public GetLeaveTypesQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository, IAppLogger<GetLeaveTypesQueryHandler> logger)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<Result<List<LeaveTypeDto>>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetLeaveTypesQuery");

            var leaveTypes = await _leaveTypeRepository.GetAllAsync();
            if (leaveTypes == null)
            {
                _logger.LogError("Failed to retrieve leave types.");
                return Result.Failure<List<LeaveTypeDto>>(LeaveTypeErrors.DataRetrievalError);
            }

            var returnData = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            if (returnData == null)
            {
                _logger.LogError("Failed to map leave types.");
                return Result.Failure<List<LeaveTypeDto>>(LeaveTypeErrors.MappingError);
            }

            _logger.LogInformation("Successfully retrieved and mapped leave types.");
            return Result<List<LeaveTypeDto>>.Success(returnData);
        }
    }
}
