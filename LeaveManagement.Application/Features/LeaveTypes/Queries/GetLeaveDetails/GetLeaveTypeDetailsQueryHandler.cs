using AutoMapper;
using FluentValidation;
using LeaveManagement.Application.Features.LeaveTypes.Dtos;
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
        private readonly IValidator<GetLeaveTypesDetailsQuery> _validator;

        public GetLeaveTypeDetailsQueryHandler(
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            IValidator<GetLeaveTypesDetailsQuery> validator)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<LeaveTypeDetailsDto>> Handle(GetLeaveTypesDetailsQuery request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                return Result.Failure<LeaveTypeDetailsDto>(LeaveTypeErrors.ValidationError(errorMessages));
            }

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
