using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Posts.Queries.GetPostById;
internal class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostDetailDto?>
{
    private readonly IPostRepository _postRepository;

    public GetPostByIdHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostDetailDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post == null)
            return null;

        return new PostDetailDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Author = new PostDetailDto.UserDto
            {
                Id = post.AuthorId,
                Username = post.Author?.Username ?? "Unknown"
            },
            Corner = new PostDetailDto.CornerDto
            {
                Id = post.CornerId,
                Name = post.Corner?.Name ?? "Unknown",
                UrlSlug = post.Corner?.UrlSlug ?? "unknown"
            },
            // CommentCount = post.Comments.Count,
            UpvoteCount = post.UpvoteCount,
            DownvoteCount = post.DownvoteCount,
            Score = post.GetScore()
        };
    }
}
