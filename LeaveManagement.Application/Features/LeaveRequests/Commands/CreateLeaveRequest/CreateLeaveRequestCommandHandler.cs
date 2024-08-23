using AutoMapper;
using LeaveManagement.Domain.EmailService;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Result<Unit>>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public CreateLeaveRequestCommandHandler(IEmailSender emailSender, IMapper mapper, ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository)
        {
            _emailSender = emailSender;
            _mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<Result<Unit>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Invalid Leave Request"));
            }

            

            
            var leaveRequest = _mapper.Map<LeaveRequest>(request);
            await _leaveRequestRepository.CreateAsync(leaveRequest);


            var email = new Email
            {
                Reciever = string.Empty, 
                MessageBody = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            var emailResult = await _emailSender.SendEmail(email);
            if (!emailResult)
            {
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Invalid Leave Request"));
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
