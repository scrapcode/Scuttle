

using MediatR;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Posts.Queries.GetPostsByCorner;

public class GetPostsByCornerHandler : IRequestHandler<GetPostsByCornerQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICornerRepository _cornerRepository;

    public GetPostsByCornerHandler(IPostRepository postRepository, ICornerRepository cornerRepository)
    {
        _postRepository = postRepository;
        _cornerRepository = cornerRepository;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetPostsByCornerQuery request, CancellationToken cancellationToken)
    {
        // Get corner by slug
        var corner = await _cornerRepository.GetByUrlSlugAsync(request.CornerSlug, cancellationToken);
        if (corner == null)
            return Enumerable.Empty<PostDto>();

        // Get posts for Corner
        var posts = await _postRepository.GetByCornerIdAsync(corner.Id, cancellationToken);

        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Author = new PostDto.UserDto
            {
                Id = p.AuthorId,
                Username = p.Author?.Username ?? "Unknown"
            },
            Corner = new PostDto.CornerDto
            {
                Id = corner.Id,
                Name = corner?.Name ?? "Unknown",
                UrlSlug = corner?.UrlSlug ?? "unknown"
            },
            // CommentCount = p.Comments.Count,
            Score = p.GetScore()
        });
    }
}
