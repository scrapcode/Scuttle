using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scuttle.Application.Corners.Commands;
using Scuttle.Application.Corners.Queries.GetAllCorners;
using Scuttle.Application.Posts.Queries.GetPostsByCorner;

namespace Scuttle.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CornersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CornersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Get all Corners
    // GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CornerDto>>> GetAllCorners()
    {
        var corners = await _mediator.Send(new GetAllCornersQuery());
        return Ok(corners);
    }

    // Create A Corner
    // POST
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateCornerResult>> CreateCorner(CreateCornerCommand command)
    {
        try
        {
            // User authentication
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Invalid user authentication.");
            }

            command.CreatorId = userId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
