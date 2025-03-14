using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scuttle.Application.Common.Exceptions;
using Scuttle.Application.Users.Commands.Login;
using Scuttle.Application.Users.Commands.Register;

namespace Scuttle.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResult>> Register(RegisterUserCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ValidationException)
        {
            return BadRequest("Invalid user data. Please check your input and try again.");
        }
        catch (ApplicationException ex) when (ex.Message.Contains("Required input is missing"))
        {
            return BadRequest("Invalid user data. Please check your input and try again.");
        }
        catch (ApplicationException ex) when (ex.Message.Contains("Username already exists"))
        {
            return Conflict("That username is already taken.");
        }
        catch (ApplicationException ex) when (ex.Message.Contains("Email already exists"))
        {
            return Conflict("There is already an account using that email.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occured. Please try again later. - {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login(LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (AuthenticationException)
        {
            return Unauthorized("Invalid credentials.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured. Please try again later.");
        }
    }
}
