﻿using AutoMapper;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
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
using LeaveManagement.Domain.Interfaces;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _appLogger;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public UpdateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper,
            IEmailSender emailSender,
            IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            this._emailSender = emailSender;
            this._appLogger = appLogger;
        }

        public async Task<Result<Unit>> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                return Result<Unit>.Failure<Unit>(LeaveRequestErrors.NotFound(request.Id));
            }

            var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                _appLogger.LogWarning("Validation failed for command: {Command}", request);
                return Result<Unit>.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Invalid Leave Request"));
            }

            _mapper.Map(request, leaveRequest);
             await _leaveRequestRepository.UpdateAsync(leaveRequest);
           

            
            var email = new Email
            {
                Reciever = string.Empty,
                MessageBody = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfully.",
                Subject = "Leave Request Updated"
            };

            bool emailSent = await _emailSender.SendEmail(email);
            if (!emailSent)
            {
                _appLogger.LogWarning("Failed to send confirmation email.");
                return Result<Unit>.Failure<Unit>(LeaveRequestErrors.EmailFailure(email.Reciever));
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
