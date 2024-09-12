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
        private readonly ICloudStorageService _cloudStorageService;  // Use Google Drive service

        public CreateLeaveRequestCommandHandler(
            IEmailSender emailSender,
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IAccessUser accessUser,
            ICloudStorageService cloudStorageService)  
        {
            _emailSender = emailSender;
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _accessUser = accessUser;
            _cloudStorageService = cloudStorageService; 
        }

        public async Task<Result<Unit>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Invalid Leave Request"));
            }

            var employeeId = _accessUser.GetUserId();

            var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            if (allocation is null)
            {
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("You do not have any allocations for this leave type."));
            }

            int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequested > allocation.NumberOfDays)
            {
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("You do not have enough days for this request."));
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(request);
            leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;

            
            if (request.File != null)
            {
                var fileName = $"{employeeId}_{Guid.NewGuid()}_{request.File.FileName}";
                using (var fileStream = request.File.OpenReadStream())
                {
                    await _cloudStorageService.UploadFileAsync(fileName, fileStream); 
                    leaveRequest.FilePath = fileName;  
                }
            }

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
                return Result.Failure<Unit>(LeaveRequestErrors.ValidationFailure("Failed to send confirmation email."));
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
