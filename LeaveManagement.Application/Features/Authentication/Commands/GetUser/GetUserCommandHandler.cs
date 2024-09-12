using LeaveManagement.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Errors;
using Identity.Authentication;
using Identity.Dtos;
using Identity.Interfaces;

namespace LeaveManagement.Application.Features.Authentication.Commands.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetUserHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ILogger<GetUserHandler> logger, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<UserDto>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
            {
                _logger.LogError("HttpContext or User is null");
                return Result.Failure<UserDto>(UserErrors.HttpContextNull());
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID not found in claims");
                return Result.Failure<UserDto>(UserErrors.UserIdNotFoundInClaims());
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User not found in the database for ID: {userId}");
                return Result.Failure<UserDto>(UserErrors.UserNotFound(userId));
            }

            
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault(); 

            
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                
                Role = role
            };

            return Result<UserDto>.Success(userDto);
        }
    }
}
