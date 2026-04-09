using FirstAppForBank.Application.Services;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryAdministrationCatalogService(InMemoryCatalogStore store) : IAdministrationCatalogService
{
    public Task<IReadOnlyCollection<CategoryAdminDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() => (IReadOnlyCollection<CategoryAdminDto>)store.Categories
            .OrderBy(x => x.Name)
            .Select(MapCategory)
            .ToArray());

        return Task.FromResult(result);
    }

    public Task<IReadOnlyCollection<ServiceAdminDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var categories = store.Categories.ToDictionary(x => x.Id, x => x.Name);
            return (IReadOnlyCollection<ServiceAdminDto>)store.Services
                .OrderBy(x => x.Name)
                .Select(service => MapService(service, categories))
                .ToArray();
        });

        return Task.FromResult(result);
    }

    public Task<CategoryAdminDto> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var category = new Domain.Models.Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                IsActive = request.IsActive
            };

            store.Categories.Add(category);
            return MapCategory(category);
        });

        return Task.FromResult(result);
    }

    public Task<CategoryAdminDto?> UpdateCategoryAsync(Guid categoryId, UpsertCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var category = store.Categories.FirstOrDefault(x => x.Id == categoryId);
            if (category is null)
            {
                return null;
            }

            category.Name = request.Name.Trim();
            category.Description = request.Description?.Trim();
            category.IsActive = request.IsActive;

            return MapCategory(category);
        });

        return Task.FromResult(result);
    }

    public Task<ServiceAdminDto> CreateServiceAsync(UpsertServiceRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var service = new Domain.Models.Service
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                CategoryId = request.CategoryId,
                Description = request.Description?.Trim(),
                Criticality = request.Criticality,
                ServiceType = request.ServiceType,
                CurrentStatus = request.Status,
                Owner = request.Owner?.Trim(),
                IsActive = request.IsActive,
                LastStatusChangedAt = DateTimeOffset.UtcNow
            };

            store.Services.Add(service);
            store.StatusHistory[service.Id] = [];
            store.Comments[service.Id] = [];

            var categories = store.Categories.ToDictionary(x => x.Id, x => x.Name);
            return MapService(service, categories);
        });

        return Task.FromResult(result);
    }

    public Task<ServiceAdminDto?> UpdateServiceAsync(Guid serviceId, UpsertServiceRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var service = store.Services.FirstOrDefault(x => x.Id == serviceId);
            if (service is null)
            {
                return null;
            }

            service.Name = request.Name.Trim();
            service.CategoryId = request.CategoryId;
            service.Description = request.Description?.Trim();
            service.Criticality = request.Criticality;
            service.ServiceType = request.ServiceType;
            service.CurrentStatus = request.Status;
            service.Owner = request.Owner?.Trim();
            service.IsActive = request.IsActive;
            service.LastStatusChangedAt = DateTimeOffset.UtcNow;

            var categories = store.Categories.ToDictionary(x => x.Id, x => x.Name);
            return MapService(service, categories);
        });

        return Task.FromResult(result);
    }

    private static CategoryAdminDto MapCategory(Domain.Models.Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        IsActive = category.IsActive
    };

    private static ServiceAdminDto MapService(Domain.Models.Service service, IReadOnlyDictionary<Guid, string> categories) => new()
    {
        Id = service.Id,
        Name = service.Name,
        CategoryId = service.CategoryId,
        CategoryName = categories.TryGetValue(service.CategoryId, out var name) ? name : "Uncategorized",
        Description = service.Description,
        Criticality = service.Criticality,
        ServiceType = service.ServiceType,
        Status = service.CurrentStatus,
        Owner = service.Owner,
        IsActive = service.IsActive
    };
}
