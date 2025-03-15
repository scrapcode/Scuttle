using MediatR;

namespace Scuttle.Application.Corners.Queries.GetAllCorners;

public class GetAllCornersQuery : IRequest<IEnumerable<CornerDto>>
{

}

public class CornerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UrlSlug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public string CreatorUsername { get; set; } = string.Empty;
}