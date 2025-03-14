using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scuttle.Domain.Entities;

public class Corner
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string UrlSlug { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public Guid CreatorId { get; private set; }
    public User Creator { get; private set; } = null!;
    public ICollection<Post> Posts { get; private set; } = new List<Post>();

    private Corner() { }

    public Corner(string name, string urlSlug, Guid creatorId)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetUrlSlug(urlSlug);
        CreatorId = creatorId;
        CreatedAt = DateTime.UtcNow;
    }

    public Corner SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Corner name cannot be empty.", nameof(name));

        Name = name;
        return this;
    }

    public Corner SetUrlSlug(string urlSlug)
    {
        if (string.IsNullOrWhiteSpace(urlSlug))
            throw new ArgumentNullException("Corner URL Slug cannot be empty.", nameof(urlSlug));

        // Validate URL slug
        if (!System.Text.RegularExpressions.Regex.IsMatch(urlSlug, "^[a-z0-9-]+$"))
            throw new ArgumentNullException("Corner URL Slug cannot be empty.", nameof(urlSlug));

        UrlSlug = urlSlug;
        return this;
    }
}
