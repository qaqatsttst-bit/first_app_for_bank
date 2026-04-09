using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Application.Services;

public sealed class AddServiceCommentRequest
{
    public string AuthorName { get; init; } = string.Empty;

    public string CommentText { get; init; } = string.Empty;
}
