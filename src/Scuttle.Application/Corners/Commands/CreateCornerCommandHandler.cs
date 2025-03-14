using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Corners.Commands;

public class CreateCornerCommandHandler : IRequestHandler<CreateCornerCommand, CreateCornerResult>
{
    private readonly ICornerRepository _cornerRepository;
    private readonly IUserRepository _userRepository;

    public CreateCornerCommandHandler(ICornerRepository cornerRepository, IUserRepository userRepository)
    {
        _cornerRepository = cornerRepository;
        _userRepository = userRepository;
    }

    public async Task<CreateCornerResult> Handle(CreateCornerCommand request, CancellationToken cancellationToken)
    {
        // Validate user
        var user = await _userRepository.GetByIdAsync(request.CreatorId);
        if (user == null)
            throw new ApplicationException($"User with the Creator Id of '{request.CreatorId}' does not exist.");

        // Generate slug if not provided
        string urlSlug = request.UrlSlug ?? GenerateUrlSlug(request.Name);

        // Check if name already exists
        if (await _cornerRepository.ExistsByNameAsync(request.Name))
            throw new ApplicationException($"A corner with the name '{request.Name}' already exists.");

        // Check if URL slug already exists
        if (await _cornerRepository.ExistsByUrlSlug(urlSlug))
            throw new ApplicationException($"A corner with the slug '{request.UrlSlug}' already exists.");

        // Create the Corner
        var corner = new Corner(request.Name, urlSlug, request.CreatorId)
            .SetDescription(request.Description);

        await _cornerRepository.AddAsync(corner, cancellationToken);

        return new CreateCornerResult
        {
            Id = corner.Id,
            Name = corner.Name,
            UrlSlug = corner.UrlSlug
        };
    }

    private string GenerateUrlSlug(string name)
    {
        // Convert to lowercase
        string slug = name.ToLower().Trim();

        // Replace spaces with hyphens
        slug = slug.Replace(" ", "-");

        // Remove special characters
        slug = Regex.Replace(slug, "[^a-z0-9-]", "");

        // Remove multiple hyphens
        slug = Regex.Replace(slug, "-+", "-");

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }
}
