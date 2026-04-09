using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Application.Services;

public sealed class UpsertServiceRequest
{
    public string Name { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public string? Description { get; init; }
    public ServiceCriticality Criticality { get; init; } = ServiceCriticality.Medium;
    public ServiceType ServiceType { get; init; } = ServiceType.Internal;
    public ServiceStatus Status { get; init; } = ServiceStatus.Unknown;
    public string? Owner { get; init; }
    public bool IsActive { get; init; } = true;
}
