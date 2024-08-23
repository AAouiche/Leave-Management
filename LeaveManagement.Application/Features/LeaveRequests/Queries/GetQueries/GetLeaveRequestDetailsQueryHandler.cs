using AutoMapper;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries
{
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler<GetLeaveRequestDetailsQuery, Result<LeaveRequestDetailsDto>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetLeaveRequestDetailsQuery> _validator;

        public GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, IValidator<GetLeaveRequestDetailsQuery> validator)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<LeaveRequestDetailsDto>> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.ValidationFailure(validationResult.Errors.First().ErrorMessage));
            }

            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithDetails(request.Id);
            if (leaveRequest == null)
            {
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.NotFound(request.Id));
            }

            var leaveRequestDetails = _mapper.Map<LeaveRequestDetailsDto>(leaveRequest);
            if (leaveRequestDetails == null)
            {
                return Result.Failure<LeaveRequestDetailsDto>(LeaveRequestErrors.MappingFailure());
            }

            return Result.Success(leaveRequestDetails);
        }
    }
}
