using LeaveManagement.Application.EmailService;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.LeaveRequests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Result<Unit>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<CancelLeaveRequestCommandHandler> _logger;

        public CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IEmailSender emailSender, IAppLogger<CancelLeaveRequestCommandHandler> logger)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._emailSender = emailSender;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Attempting to cancel leave request with ID {request.Id}.");

            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                _logger.LogWarning($"Leave request with ID {request.Id} not found.");
                return Result.Failure<Unit>(LeaveRequestErrors.NotFound(request.Id));
            }

            leaveRequest.Cancelled = true;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            _logger.LogInformation($"Leave request with ID {request.Id} cancelled successfully. Sending confirmation email.");

            var email = new Email
            {
                Reciever = string.Empty,
                MessageBody = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} " +
                              $"has been cancelled successfully.",
                Subject = "Leave Request Cancelled"
            };

            bool emailSent = await _emailSender.SendEmail(email);
            if (!emailSent)
            {
                _logger.LogWarning($"Failed to send cancellation confirmation email for leave request ID {request.Id}.");
                return Result.Failure<Unit>(LeaveRequestErrors.EmailFailure(email.Reciever));
            }

            _logger.LogInformation($"Confirmation email sent for cancelled leave request ID {request.Id}.");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
