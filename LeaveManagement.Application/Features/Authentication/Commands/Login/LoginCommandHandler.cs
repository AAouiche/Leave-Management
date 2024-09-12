using FluentValidation;
using FluentValidation.Results;

using LeaveManagement.Shared.Common;

using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Errors;
using Identity.Authentication;
using Identity.Dtos;
using LeaveManagement.Identity.Interfaces;

namespace LeaveManagement.Application.Features.Authentication.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<LoginCommand> _validator;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, IValidator<LoginCommand> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<Result<UserDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            
            ValidationResult validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.ValidationFailure(validationResult.Errors));
            }

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.UserNotFound(command.Email));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, command.Password);
            if (!isPasswordValid)
            {
                return Result.Failure<UserDto>(AuthenticationErrors.InvalidCredentials());
            }

            var loggedInUser = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = _tokenService.GenerateToken(user)
            };

            return Result<UserDto>.Success(loggedInUser);
        }
    }
}
