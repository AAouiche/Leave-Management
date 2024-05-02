using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Domain.LeaveTypes;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : BaseApiController
    {
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetLeaveTypesQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetLeaveTypesDetailsQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(LeaveType leaveType)
        {
            if (leaveType == null)
            {
                return BadRequest("Invalid data.");
            }
            var command = new CreateLeaveTypeCommand { Name = leaveType.Name , Days= leaveType.Days};
            var result = await Mediator.Send(command);
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
        public async Task<IActionResult> Update(LeaveType leaveType)
        {
            if (leaveType == null )
            {
                return BadRequest("Invalid data.");
            }

            var command = new UpdateLeaveTypeCommand { Days = leaveType.Days, Name = leaveType.Name, Id = leaveType.Id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
