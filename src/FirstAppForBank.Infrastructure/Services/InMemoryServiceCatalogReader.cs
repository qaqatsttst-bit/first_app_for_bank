using FirstAppForBank.Application.Services;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryServiceCatalogReader(InMemoryCatalogStore store) : IServiceCatalogReader
{
    public Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var categories = store.Categories.ToDictionary(x => x.Id, x => x.Name);
            return (IReadOnlyCollection<ServiceCardDto>)store.Services
                .OrderBy(x => categories.TryGetValue(x.CategoryId, out var category) ? category : string.Empty)
                .ThenBy(x => x.Name)
                .Select(service => new ServiceCardDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Category = categories.TryGetValue(service.CategoryId, out var categoryName) ? categoryName : "Uncategorized",
                    Status = service.CurrentStatus.ToString(),
                    Criticality = service.Criticality.ToString(),
                    ServiceType = service.ServiceType.ToString(),
                    Owner = service.Owner,
                    LastStatusChangedAt = service.LastStatusChangedAt
                })
                .ToArray();
        });

        return Task.FromResult(result);
    }

    public Task<ServiceDetailsDto?> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var service = store.Services.FirstOrDefault(x => x.Id == serviceId);
            if (service is null)
            {
                return null;
            }

            var categoryName = store.Categories.FirstOrDefault(x => x.Id == service.CategoryId)?.Name ?? "Uncategorized";
            store.StatusHistory.TryGetValue(service.Id, out var history);
            store.Comments.TryGetValue(service.Id, out var comments);

            return new ServiceDetailsDto
            {
                Id = service.Id,
                Name = service.Name,
                Category = categoryName,
                Description = service.Description ?? string.Empty,
                Status = service.CurrentStatus.ToString(),
                Criticality = service.Criticality.ToString(),
                ServiceType = service.ServiceType.ToString(),
                Owner = service.Owner,
                RunbookUrl = service.RunbookUrl,
                DashboardUrl = service.DashboardUrl,
                Notes = service.Notes,
                LastStatusChangedAt = service.LastStatusChangedAt,
                StatusHistory = (history ?? []).OrderByDescending(x => x.ChangedAt).ToArray(),
                Comments = (comments ?? []).OrderByDescending(x => x.CreatedAt).ToArray()
            };
        });

        return Task.FromResult(result);
    }
}
