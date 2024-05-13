using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : BaseApiController
    {
       
        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDto>>> GetAll()
        {
            var query = new GetLeaveTypesQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDetailsDto>> Get(int id)
        {
            var query = new GetLeaveTypesDetailsQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveTypeCommand leaveType)
        {
            if (leaveType == null)
            {
                return BadRequest("Invalid data.");
            }
            
            var result = await Mediator.Send(leaveType);
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
        public async Task<ActionResult<int>> Update(UpdateLeaveTypeCommand leaveType)
        {
            if (leaveType == null )
            {
                return BadRequest("Invalid data.");
            }

            
            var result = await Mediator.Send(leaveType);
            return HandleResult(result);
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
