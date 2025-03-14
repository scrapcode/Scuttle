

using MediatR;

namespace Scuttle.Application.Corners.Commands;

public class CreateCornerCommand : IRequest<CreateCornerResult>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? UrlSlug { get; set; } = string.Empty;
    public Guid CreatorId { get; set; }
}

public class CreateCornerResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UrlSlug { get; set; } = string.Empty;
}