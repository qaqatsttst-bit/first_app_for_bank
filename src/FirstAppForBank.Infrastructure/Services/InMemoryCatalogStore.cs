using FirstAppForBank.Application.Services;
using FirstAppForBank.Domain.Enums;
using FirstAppForBank.Domain.Models;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryCatalogStore
{
    private readonly object syncRoot = new();

    public List<Category> Categories { get; }

    public List<Service> Services { get; }

    public Dictionary<Guid, List<ServiceStatusHistoryItemDto>> StatusHistory { get; }

    public Dictionary<Guid, List<ServiceCommentDto>> Comments { get; }

    public InMemoryCatalogStore()
    {
        var externalPaymentsCategoryId = Guid.Parse("aaaaaaaa-1111-2222-3333-aaaaaaaaaaaa");
        var internalCoreCategoryId = Guid.Parse("bbbbbbbb-1111-2222-3333-bbbbbbbbbbbb");
        var riskCategoryId = Guid.Parse("cccccccc-1111-2222-3333-cccccccccccc");

        var koronapayId = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111");
        var paymentRoutingId = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222");
        var fraudCheckId = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333");

        Categories =
        [
            new Category { Id = externalPaymentsCategoryId, Name = "External Payments", Description = "Внешние платежные направления и провайдеры.", IsActive = true },
            new Category { Id = internalCoreCategoryId, Name = "Internal Payment Core", Description = "Ключевые внутренние сервисы платежного ядра.", IsActive = true },
            new Category { Id = riskCategoryId, Name = "Risk & Control", Description = "Сервисы контроля и антифрода.", IsActive = true }
        ];

        Services =
        [
            new Service
            {
                Id = koronapayId,
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
                IsActive = true,
                LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-12)
            },
            new Service
            {
                Id = paymentRoutingId,
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
                IsActive = true,
                LastStatusChangedAt = DateTimeOffset.UtcNow.AddMinutes(-27)
            },
            new Service
            {
                Id = fraudCheckId,
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
                IsActive = true,
                LastStatusChangedAt = DateTimeOffset.UtcNow.AddHours(-2)
            }
        ];

        StatusHistory = new Dictionary<Guid, List<ServiceStatusHistoryItemDto>>
        {
            [koronapayId] =
            [
                new ServiceStatusHistoryItemDto
                {
                    OldStatus = "Degraded",
                    NewStatus = "Ok",
                    ChangeSource = "Prometheus",
                    ChangeSourceType = "integration",
                    Comment = "Метрики стабилизировались, error rate вернулся к норме.",
                    ChangedAt = DateTimeOffset.UtcNow.AddMinutes(-12)
                }
            ],
            [paymentRoutingId] =
            [
                new ServiceStatusHistoryItemDto
                {
                    OldStatus = "Ok",
                    NewStatus = "Degraded",
                    ChangeSource = "Alertmanager",
                    ChangeSourceType = "integration",
                    Comment = "Сработал алерт по росту latency на критическом маршруте.",
                    ChangedAt = DateTimeOffset.UtcNow.AddMinutes(-27)
                }
            ],
            [fraudCheckId] =
            [
                new ServiceStatusHistoryItemDto
                {
                    OldStatus = "Ok",
                    NewStatus = "Unknown",
                    ChangeSource = "System",
                    ChangeSourceType = "system",
                    Comment = "Источник телеметрии временно не отвечает, данные считаются устаревшими.",
                    ChangedAt = DateTimeOffset.UtcNow.AddHours(-2)
                }
            ]
        };

        Comments = new Dictionary<Guid, List<ServiceCommentDto>>
        {
            [koronapayId] =
            [
                new ServiceCommentDto
                {
                    AuthorName = "Алексей Иванов",
                    CommentText = "Проверили внешний контур, сервис работает штатно после кратковременной деградации.",
                    CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10)
                }
            ],
            [paymentRoutingId] =
            [
                new ServiceCommentDto
                {
                    AuthorName = "Мария Петрова",
                    CommentText = "Нужно проверить узкий участок маршрутизации и сравнить динамику за последние 30 минут.",
                    CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-20)
                }
            ],
            [fraudCheckId] =
            [
                new ServiceCommentDto
                {
                    AuthorName = "Дежурный инженер",
                    CommentText = "Ожидаем восстановление сигнала от внешнего источника, инцидент пока не подтвержден как Down.",
                    CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-95)
                }
            ]
        };
    }

    public T ExecuteLocked<T>(Func<T> action)
    {
        lock (syncRoot)
        {
            return action();
        }
    }
}
