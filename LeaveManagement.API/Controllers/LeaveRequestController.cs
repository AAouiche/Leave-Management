using LeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.ChangeLeaveRequestApproval;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestDetails;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : BaseApiController
    {
        [HttpGet("getall")]
        public async Task<ActionResult<List<LeaveRequestListDto>>> GetAll()
        {
            var query = new GetAllLeaveRequestQuery();
            var result = await Mediator.Send(query);
            return HandleResult<List<LeaveRequestListDto>>(result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<LeaveRequestDetailsDto>> Get(int id)
        {
            var query = new GetLeaveRequestDetailsQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLeaveRequestCommand leaveRequest)
        {
            if (leaveRequest == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await Mediator.Send(leaveRequest);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Unit>> Update(UpdateLeaveRequestCommand leaveRequest)
        {
            if (leaveRequest == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await Mediator.Send(leaveRequest);
            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("cancel/{id}")]
        public async Task<ActionResult<Unit>> Cancel(int id)
        {
            var command = new CancelLeaveRequestCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("approve/{id}")]
        public async Task<ActionResult<Unit>> ChangeApproval([FromBody] ChangeLeaveRequestApprovalCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
