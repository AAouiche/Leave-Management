using LeaveManagement.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<BaseApiController> _logger;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Value != null)
                {
                    return Ok(result.Value);
                }
                return NoContent();
            }
            else
            {
                return result.ToProblemDetails();
            }
        }
    }
}
