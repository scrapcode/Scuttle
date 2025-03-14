using System.Runtime.CompilerServices;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scuttle.Application.Posts.Commands.CreatePost;
using Scuttle.Application.Posts.Queries.GetPostById;
using Scuttle.Application.Posts.Queries.GetPostsByCorner;

namespace Scuttle.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Get a Post by ID
    // GET /{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDetailDto>> GetPost(Guid id)
    {
        var post = await _mediator.Send(new GetPostByIdQuery { Id = id });
        if (post == null)
            return NotFound();

        return Ok(post);
    }

    // Get Posts by cornerSlug
    // GET corner/{cornerSlug}
    [HttpGet("corner/{cornerSlug}")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByCorner(string cornerSlug)
    {
        var posts = await _mediator.Send(new GetPostsByCornerQuery { CornerSlug = cornerSlug });

        return Ok(posts);
    }

    // Create a Post
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreatePostResult>> CreatePost(CreatePostCommand command)
    {
        try
        {
            // User authentication
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Invalid user authentication.");
            }

            command.AuthorId = userId;

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
