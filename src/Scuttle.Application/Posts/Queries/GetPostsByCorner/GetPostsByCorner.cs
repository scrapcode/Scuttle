using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Scuttle.Application.Posts.Queries.GetPostsByCorner;
public class GetPostsByCornerQuery : IRequest<IEnumerable<PostDto>>
{
    public string CornerSlug { get; set; } = string.Empty;
}

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserDto Author { get; set; } = null!;
    public CornerDto Corner { get; set; } = null!;
    public int CommentCount { get; set; }
    public int Score { get; set; }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class CornerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UrlSlug { get; set; } = string.Empty;
    }
}
