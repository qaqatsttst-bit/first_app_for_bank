namespace FirstAppForBank.Application.Services;

public sealed class ServiceCommentDto
{
    public string AuthorName { get; init; } = string.Empty;

    public string CommentText { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }
}
