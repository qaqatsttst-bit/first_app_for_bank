namespace FirstAppForBank.Application.Services;

public sealed class ServiceDetailsDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string Criticality { get; init; } = string.Empty;

    public string ServiceType { get; init; } = string.Empty;

    public string? Owner { get; init; }

    public string? RunbookUrl { get; init; }

    public string? DashboardUrl { get; init; }

    public string? Notes { get; init; }

    public DateTimeOffset LastStatusChangedAt { get; init; }
}
