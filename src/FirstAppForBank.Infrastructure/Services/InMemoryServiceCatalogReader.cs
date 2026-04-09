using FirstAppForBank.Application.Services;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryServiceCatalogReader : IServiceCatalogReader
{
    private static readonly IReadOnlyCollection<ServiceDetailsDto> Services =
    [
        new ServiceDetailsDto
        {
            Id = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"),
            Name = "KoronaPay",
            Category = "External Payments",
            Description = "Внешний платежный сервис для обработки операций по международным и внешним направлениям.",
            Status = "Ok",
            Criticality = "Critical",
            ServiceType = "External",
            Owner = "Payments Team",
            RunbookUrl = "https://runbook.local/koronapay",
            DashboardUrl = "https://grafana.local/koronapay",
            Notes = "Сервис критичен для внешнего платежного контура.",
            LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-12)
        },
        new ServiceDetailsDto
        {
            Id = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222"),
            Name = "Payment Routing",
            Category = "Internal Payment Core",
            Description = "Внутренний сервис маршрутизации платежей между системами банка и внешними шлюзами.",
            Status = "Degraded",
            Criticality = "High",
            ServiceType = "Internal",
            Owner = "Core Banking Team",
            RunbookUrl = "https://runbook.local/payment-routing",
            DashboardUrl = "https://grafana.local/payment-routing",
            Notes = "Наблюдается рост latency и нестабильность части запросов.",
            LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-27)
        },
        new ServiceDetailsDto
        {
            Id = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333"),
            Name = "Fraud Check Gateway",
            Category = "Risk & Control",
            Description = "Интеграционный сервис, отвечающий за передачу платежных событий в контур антифрода.",
            Status = "Unknown",
            Criticality = "High",
            ServiceType = "Integration",
            Owner = "Risk Platform Team",
            RunbookUrl = "https://runbook.local/fraud-check",
            DashboardUrl = "https://grafana.local/fraud-check",
            Notes = "Последнее обновление сигналов устарело, требуется проверка источника данных.",
            LastStatusChangedAt = DateTimeOffset.UtcNow.AddHours(-2)
        }
    ];

    public Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        var cards = Services
            .Select(service => new ServiceCardDto
            {
                Id = service.Id,
                Name = service.Name,
                Category = service.Category,
                Status = service.Status,
                Criticality = service.Criticality,
                ServiceType = service.ServiceType,
                Owner = service.Owner,
                LastStatusChangedAt = service.LastStatusChangedAt
            })
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<ServiceCardDto>>(cards);
    }

    public Task<ServiceDetailsDto?> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        var service = Services.FirstOrDefault(x => x.Id == serviceId);
        return Task.FromResult(service);
    }
}
