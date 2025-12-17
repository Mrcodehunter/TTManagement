using MediatR;
using Microsoft.AspNetCore.Mvc;
using TTManagement.Features.Auth;

namespace TTManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { Token = token });
    }
}

