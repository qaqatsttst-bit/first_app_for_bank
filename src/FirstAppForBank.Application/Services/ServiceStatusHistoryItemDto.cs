namespace FirstAppForBank.Application.Services;

public sealed class ServiceStatusHistoryItemDto
{
    public string? OldStatus { get; init; }

    public string NewStatus { get; init; } = string.Empty;

    public string? ChangeSource { get; init; }

    public string ChangeSourceType { get; init; } = string.Empty;

    public string? Comment { get; init; }

    public DateTimeOffset ChangedAt { get; init; }
}
