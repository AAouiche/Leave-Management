using LeaveManagement.Application.Features.Authentication.Commands.GetAllUsers;
using LeaveManagement.Application.Features.Authentication.Commands.GetUser;
using LeaveManagement.Application.Features.Authentication.Commands;

using Microsoft.AspNetCore.Mvc;
using LeaveManagement.Infrastructure.Services;
using LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Identity.Dtos;
using LeaveManagement.Identity.Interfaces;
using LeaveManagement.Application.Features.Authentication.Commands.Register;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService, ILogger<UserController> logger)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        {
            var result = await Mediator.Send(loginCommand);
            return HandleResult(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                return Unauthorized(new { message = "Token is required." });
            }
            _tokenService.ValidateToken(new TokenDTO { Token = token });

            var result = await Mediator.Send(new GetUserCommand());
            return HandleResult(result);
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await Mediator.Send(new GetAllUsersCommand());
            return HandleResult(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand registerCommand)
        {
            var result = await Mediator.Send(registerCommand);
            return HandleResult(result);
        }
    }
}
