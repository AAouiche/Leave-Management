using AutoMapper;
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
using LeaveManagement.Domain.LeaveAllocations;

using LeaveManagement.Domain.Interfaces;
using Identity.Interfaces;
using LeaveManagement.Application.Interfaces;
using LeaveManagement.Application.Logging;

namespace LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Result<Unit>>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAccessUser _accessUser;
        private readonly IS3Service _s3Service;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger; 

        public CreateLeaveRequestCommandHandler(
            IEmailSender emailSender,
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IAccessUser accessUser,
            IS3Service s3Service,
            IAppLogger<CreateLeaveRequestCommandHandler> logger)  
        {
            _emailSender = emailSender;
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _accessUser = accessUser;
            _s3Service = s3Service;
            _logger = logger; 
        }

        public async Task<Result<Unit>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateLeaveRequestCommand for user ID: {UserId}", _accessUser.GetUserId());

            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.Errors);
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Invalid Leave Request"));
            }

            var employeeId = _accessUser.GetUserId();

            var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            if (allocation is null)
            {
                _logger.LogWarning("No allocations found for user ID: {UserId}, LeaveTypeId: {LeaveTypeId}", employeeId, request.LeaveTypeId);
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("You do not have any allocations for this leave type."));
            }

            int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequested > allocation.NumberOfDays)
            {
                _logger.LogWarning("Insufficient leave days for user ID: {UserId}. Requested: {DaysRequested}, Available: {DaysAvailable}", employeeId, daysRequested, allocation.NumberOfDays);
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("You do not have enough days for this request."));
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(request);
            leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;

            if (request.File != null)
            {
                _logger.LogInformation("Uploading file for user ID: {UserId}", employeeId);
                var fileKeyResult = await _s3Service.UploadFileAsync(request.File);

                if (string.IsNullOrEmpty(fileKeyResult))
                {
                    _logger.LogWarning("File upload failed for user ID: {UserId}", employeeId);
                    return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Failed to upload the file."));
                }

                leaveRequest.FilePath = fileKeyResult;
                _logger.LogInformation("File uploaded successfully. S3 Key: {FileKey}", fileKeyResult);
            }

            await _leaveRequestRepository.CreateAsync(leaveRequest);
            _logger.LogInformation("Leave request created successfully for user ID: {UserId}", employeeId);

            var email = new Email
            {
                Reciever = string.Empty, 
                MessageBody = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            var emailResult = await _emailSender.SendEmail(email);
            if (!emailResult)
            {
                _logger.LogWarning("Failed to send confirmation email to user ID: {UserId}", employeeId);
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Failed to send confirmation email."));
            }

            _logger.LogInformation("Confirmation email sent successfully to user ID: {UserId}", employeeId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}