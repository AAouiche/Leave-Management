using LeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Dtos;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;
using LeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocationDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : BaseApiController
    {
        [HttpGet("getall")]
        public async Task<ActionResult<List<LeaveAllocationDto>>> GetAll()
        {
            var query = new GetAllLeaveAllocationQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<LeaveAllocationDetailsDto>> Get(int id)
        {
            var query = new GetLeaveAllocationDetailsQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLeaveAllocationCommand leaveAllocation)
        {
            if (leaveAllocation == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await Mediator.Send(leaveAllocation);
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
        public async Task<ActionResult<Unit>> Update( UpdateLeaveAllocationCommand leaveAllocation)
        {
            if (leaveAllocation == null)
            {
                return BadRequest("Invalid data.");
            }

            
            var result = await Mediator.Send(leaveAllocation);
            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var command = new DeleteLeaveAllocationCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
