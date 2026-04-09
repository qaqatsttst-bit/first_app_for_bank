using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Domain.Models;

public sealed class Service
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public ServiceCriticality Criticality { get; set; } = ServiceCriticality.Medium;

    public ServiceType ServiceType { get; set; } = ServiceType.Internal;

    public ServiceStatus CurrentStatus { get; set; } = ServiceStatus.Unknown;

    public string? Owner { get; set; }

    public string? RunbookUrl { get; set; }

    public string? DashboardUrl { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset LastStatusChangedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<ServiceStatusHistory> StatusHistory { get; init; } = new List<ServiceStatusHistory>();

    public ICollection<ServiceComment> Comments { get; init; } = new List<ServiceComment>();
}
