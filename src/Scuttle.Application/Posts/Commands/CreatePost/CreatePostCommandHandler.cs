using MediatR;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Posts.Commands.CreatePost;
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostResult>
{
    private readonly IPostRepository _postRepository;
    private readonly ICornerRepository _cornerRepository;
    private readonly IUserRepository _userRepository;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        ICornerRepository cornerRepository,
        IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _cornerRepository = cornerRepository;
        _userRepository = userRepository;
    }

    public async Task<CreatePostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        // Ensure Corner exists
        var corner = await _cornerRepository.GetByIdAsync(request.CornerId, cancellationToken);
        if (corner == null)
            throw new ApplicationException($"Corner with id '{request.CornerId}' does not exist.");

        // Ensure User exists
        var user = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (user == null)
            throw new ApplicationException($"User with id '{request.AuthorId}' does not exist.");

        // Create the Post
        var post = new Post(request.Title, request.Content, request.AuthorId, request.CornerId);

        return new CreatePostResult
        {
            Id = post.Id,
            Title = post.Title,
            CornerName = corner.Name,
            CornerSlug = corner.UrlSlug
        };
    }
}
