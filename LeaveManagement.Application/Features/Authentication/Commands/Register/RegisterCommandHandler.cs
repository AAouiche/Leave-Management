using FluentValidation;
using FluentValidation.Results;
using Identity.Authentication;
using Identity.Dtos;
using Identity.Errors;
using LeaveManagement.Identity.Interfaces;
using LeaveManagement.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, IValidator<RegisterCommand> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<Result<UserDto>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            
            ValidationResult validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.ValidationFailure(validationResult.Errors));
            }

            
            var existingUser = await _userManager.FindByEmailAsync(command.Email);
            if (existingUser != null)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.EmailAlreadyInUse(command.Email));
            }

            
            var newUser = new ApplicationUser
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.Email
            };

            var result = await _userManager.CreateAsync(newUser, command.Password);
            if (!result.Succeeded)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.UserCreationFailed(result.Errors.Select(e => e.Description)));
            }

            
            var userDto = new UserDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Token = _tokenService.GenerateToken(newUser)
            };

            return Result<UserDto>.Success(userDto);
        }
    }
}
