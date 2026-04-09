using FirstAppForBank.Domain.Enums;
using FirstAppForBank.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAppForBank.Infrastructure.Persistence;

public sealed class AppDbContextSeed(AppDbContext dbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.Categories.AnyAsync(cancellationToken) || await dbContext.Services.AnyAsync(cancellationToken))
        {
            return;
        }

        var externalPaymentsCategoryId = Guid.Parse("aaaaaaaa-1111-2222-3333-aaaaaaaaaaaa");
        var internalCoreCategoryId = Guid.Parse("bbbbbbbb-1111-2222-3333-bbbbbbbbbbbb");
        var riskCategoryId = Guid.Parse("cccccccc-1111-2222-3333-cccccccccccc");

        var categories =
            new[]
            {
                new Category
                {
                    Id = externalPaymentsCategoryId,
                    Name = "External Payments",
                    Description = "Внешние платежные направления и провайдеры."
                },
                new Category
                {
                    Id = internalCoreCategoryId,
                    Name = "Internal Payment Core",
                    Description = "Ключевые внутренние сервисы платежного ядра."
                },
                new Category
                {
                    Id = riskCategoryId,
                    Name = "Risk & Control",
                    Description = "Сервисы контроля и антифрода."
                }
            };

        var services =
            new[]
            {
                new Service
                {
                    Id = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"),
                    Name = "KoronaPay",
                    CategoryId = externalPaymentsCategoryId,
                    Description = "Внешний платежный сервис для обработки операций по международным и внешним направлениям.",
                    CurrentStatus = ServiceStatus.Ok,
                    Criticality = ServiceCriticality.Critical,
                    ServiceType = ServiceType.External,
                    Owner = "Payments Team",
                    RunbookUrl = "https://runbook.local/koronapay",
                    DashboardUrl = "https://grafana.local/koronapay",
                    Notes = "Сервис критичен для внешнего платежного контура.",
                    LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-12)
                },
                new Service
                {
                    Id = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222"),
                    Name = "Payment Routing",
                    CategoryId = internalCoreCategoryId,
                    Description = "Внутренний сервис маршрутизации платежей между системами банка и внешними шлюзами.",
                    CurrentStatus = ServiceStatus.Degraded,
                    Criticality = ServiceCriticality.High,
                    ServiceType = ServiceType.Internal,
                    Owner = "Core Banking Team",
                    RunbookUrl = "https://runbook.local/payment-routing",
                    DashboardUrl = "https://grafana.local/payment-routing",
                    Notes = "Наблюдается рост latency и нестабильность части запросов.",
                    LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-27)
                },
                new Service
                {
                    Id = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333"),
                    Name = "Fraud Check Gateway",
                    CategoryId = riskCategoryId,
                    Description = "Интеграционный сервис, отвечающий за передачу платежных событий в контур антифрода.",
                    CurrentStatus = ServiceStatus.Unknown,
                    Criticality = ServiceCriticality.High,
                    ServiceType = ServiceType.Integration,
                    Owner = "Risk Platform Team",
                    RunbookUrl = "https://runbook.local/fraud-check",
                    DashboardUrl = "https://grafana.local/fraud-check",
                    Notes = "Последнее обновление сигналов устарело, требуется проверка источника данных.",
                    LastStatusChangedAt = DateTimeOffset.UtcNow.AddHours(-2)
                }
            };

        await dbContext.Categories.AddRangeAsync(categories, cancellationToken);
        await dbContext.Services.AddRangeAsync(services, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
