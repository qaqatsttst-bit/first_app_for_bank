using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Domain.Models;

public sealed class ServiceStatusHistory
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid ServiceId { get; set; }

    public Service? Service { get; set; }

    public ServiceStatus? OldStatus { get; set; }

    public ServiceStatus NewStatus { get; set; }

    public string? ChangeSource { get; set; }

    public string ChangeSourceType { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}
