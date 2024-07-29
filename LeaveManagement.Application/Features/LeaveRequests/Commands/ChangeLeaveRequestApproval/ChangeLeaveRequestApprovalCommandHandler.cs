using AutoMapper;
using LeaveManagement.Application.EmailService;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
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

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public ChangeLeaveRequestApprovalCommandHandler(
             ILeaveRequestRepository leaveRequestRepository,
             ILeaveTypeRepository leaveTypeRepository,
             IMapper mapper,
             IEmailSender emailSender)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<Result<Unit>> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                return Result.Failure<Unit>(LeaveRequestErrors.NotFound(request.Id));
            }

            leaveRequest.Approved = request.Approved;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            var email = new Email
            {
                Reciever = string.Empty,
                MessageBody = $"Your leave request has been {(request.Approved ? "approved" : "denied")}.",
                Subject = "Leave Request Approval Status Changed"
            };

            var emailResult = await _emailSender.SendEmail(email);
            if (!emailResult)
            {
                return Result.Failure<Unit>(LeaveRequestErrors.EmailFailure(email.Reciever));
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
