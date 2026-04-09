namespace FirstAppForBank.Domain.Models;

public sealed class ServiceComment
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid ServiceId { get; set; }

    public Service? Service { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string CommentText { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
