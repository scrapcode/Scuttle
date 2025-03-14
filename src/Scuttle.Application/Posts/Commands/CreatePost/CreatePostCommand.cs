

using MediatR;

namespace Scuttle.Application.Posts.Commands.CreatePost;

public class CreatePostCommand : IRequest<CreatePostResult>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CornerId { get; set; }
    public Guid AuthorId { get; set; }
}

public class CreatePostResult
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CornerName { get; set; } = string.Empty;
    public string CornerSlug { get; set; } = string.Empty;
}