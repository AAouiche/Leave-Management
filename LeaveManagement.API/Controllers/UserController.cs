using LeaveManagement.Application.Features.Authentication.Commands.GetAllUsers;
using LeaveManagement.Application.Features.Authentication.Commands.GetUser;
using LeaveManagement.Application.Features.Authentication.Commands;
using LeaveManagement.Domain.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        {
            if (loginCommand == null)
            {
                _logger.LogError("Login data is null.");
                return BadRequest("Invalid login data.");
            }

            var result = await Mediator.Send(loginCommand);
            return HandleResult(result);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var result = await Mediator.Send(new GetUserCommand());
            return HandleResult(result);
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await Mediator.Send(new GetAllUsersCommand());
            return HandleResult(result);
        }
    }
}
