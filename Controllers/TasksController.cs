using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TTManagement.Entities;
using TTManagement.Features.ProjectTasks;

namespace TTManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Create(CreateTaskCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetTasksQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Update(int id, UpdateTaskCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Employee,Manager,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ProjectTaskStatus status)
    {
        var command = new UpdateTaskStatusCommand(id, status);
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }
}

