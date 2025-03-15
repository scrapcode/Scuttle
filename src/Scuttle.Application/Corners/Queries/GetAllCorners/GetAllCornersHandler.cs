

using MediatR;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Corners.Queries.GetAllCorners;

public class GetAllCornersHandler : IRequestHandler<GetAllCornersQuery, IEnumerable<CornerDto>>
{
    private readonly ICornerRepository _cornerRepository;
    private readonly IUserRepository _userRepository;

    public GetAllCornersHandler(ICornerRepository cornerRepository, IUserRepository userRepository)
    {
        _cornerRepository = cornerRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CornerDto>> Handle(GetAllCornersQuery request, CancellationToken cancellationToken)
    {
        var corners = await _cornerRepository.GetAllAsync(cancellationToken);

        return corners.Select(c => new CornerDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            UrlSlug = c.UrlSlug,
            CreatedAt = c.CreatedAt,
            CreatorId = c.CreatorId,
            CreatorUsername = c.Creator?.Username ?? "Unknown"
        });
    }
}
