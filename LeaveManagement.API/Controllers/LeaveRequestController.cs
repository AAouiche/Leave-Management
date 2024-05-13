using LeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.ChangeLeaveRequestApproval;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetAllQueries;
using LeaveManagement.Application.Features.LeaveRequests.Queries.GetQueries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestListDto>>> GetAll()
        {
            var query = new GetAllLeaveRequestQuery();
            var result = await Mediator.Send(query);
            return HandleResult<List<LeaveRequestListDto>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDetailsDto>> Get(int id)
        {
            var query = new GetLeaveRequestDetailsQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, UpdateLeaveRequestCommand leaveRequest)
        {
            if (leaveRequest == null || id != leaveRequest.Id)
            {
                return BadRequest("Invalid data.");
            }

            leaveRequest.Id = id; // Ensure the ID from the route is used
            var result = await Mediator.Send(leaveRequest);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        // Endpoint to cancel a leave request
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var command = new CancelLeaveRequestCommand { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        // Endpoint to change the approval status of a leave request
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ChangeApproval([FromBody] ChangeLeaveRequestApprovalCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
