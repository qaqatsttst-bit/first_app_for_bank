using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Application.Services;

public sealed class ServiceAdminDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ServiceCriticality Criticality { get; init; }
    public ServiceType ServiceType { get; init; }
    public ServiceStatus Status { get; init; }
    public string? Owner { get; init; }
    public bool IsActive { get; init; }
}
