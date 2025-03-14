using System;
using System.Xml.Linq;

namespace Scuttle.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid AuthorId { get; private set; }
    public User Author { get; private set; } = null!;
    public Guid CornerId { get; private set; }
    public Corner Corner { get; private set; } = null!;
    //public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public int UpvoteCount { get; private set; } = 0;
    public int DownvoteCount { get; private set; } = 0;

    // For EF Core
    private Post() { }

    public Post(string title, string content, Guid authorId, Guid cornerId)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        SetContent(content);
        AuthorId = authorId;
        CornerId = cornerId;
        CreatedAt = DateTime.UtcNow;
    }

    public Post SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Post SetContent(string content)
    {
        Content = content ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Post AddUpvote()
    {
        UpvoteCount++;
        return this;
    }

    public Post AddDownvote()
    {
        DownvoteCount++;
        return this;
    }

    public Post RemoveUpvote()
    {
        if (UpvoteCount > 0)
            UpvoteCount--;
        return this;
    }

    public Post RemoveDownvote()
    {
        if (DownvoteCount > 0)
            DownvoteCount--;
        return this;
    }

    public int GetScore() => UpvoteCount - DownvoteCount;
}