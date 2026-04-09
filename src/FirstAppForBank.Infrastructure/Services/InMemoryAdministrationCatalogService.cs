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

            var previousStatus = service.CurrentStatus;

            service.Name = request.Name.Trim();
            service.CategoryId = request.CategoryId;
            service.Description = request.Description?.Trim();
            service.Criticality = request.Criticality;
            service.ServiceType = request.ServiceType;
            service.CurrentStatus = request.Status;
            service.Owner = request.Owner?.Trim();
            service.IsActive = request.IsActive;
            service.LastStatusChangedAt = DateTimeOffset.UtcNow;

            if (previousStatus != request.Status)
            {
                if (!store.StatusHistory.TryGetValue(service.Id, out var history))
                {
                    history = [];
                    store.StatusHistory[service.Id] = history;
                }

                history.Add(new ServiceStatusHistoryItemDto
                {
                    OldStatus = previousStatus.ToString(),
                    NewStatus = request.Status.ToString(),
                    ChangeSource = "Administration",
                    ChangeSourceType = "manual",
                    Comment = "Статус изменен из раздела управления сервисами.",
                    ChangedAt = DateTimeOffset.UtcNow
                });
            }

            var categories = store.Categories.ToDictionary(x => x.Id, x => x.Name);
            return MapService(service, categories);
        });

        return Task.FromResult(result);
    }

    public Task<ServiceCommentDto?> AddCommentAsync(Guid serviceId, AddServiceCommentRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            if (!store.Services.Any(x => x.Id == serviceId))
            {
                return null;
            }

            if (!store.Comments.TryGetValue(serviceId, out var comments))
            {
                comments = [];
                store.Comments[serviceId] = comments;
            }

            var comment = new ServiceCommentDto
            {
                AuthorName = request.AuthorName.Trim(),
                CommentText = request.CommentText.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            comments.Add(comment);
            return comment;
        });

        return Task.FromResult(result);
    }

    public Task<ServiceStatusHistoryItemDto?> ChangeStatusAsync(Guid serviceId, ChangeServiceStatusRequest request, CancellationToken cancellationToken = default)
    {
        var result = store.ExecuteLocked(() =>
        {
            var service = store.Services.FirstOrDefault(x => x.Id == serviceId);
            if (service is null)
            {
                return null;
            }

            if (!store.StatusHistory.TryGetValue(serviceId, out var history))
            {
                history = [];
                store.StatusHistory[serviceId] = history;
            }

            var previousStatus = service.CurrentStatus;
            service.CurrentStatus = request.NewStatus;
            service.LastStatusChangedAt = DateTimeOffset.UtcNow;

            var item = new ServiceStatusHistoryItemDto
            {
                OldStatus = previousStatus.ToString(),
                NewStatus = request.NewStatus.ToString(),
                ChangeSource = request.ChangeSource,
                ChangeSourceType = request.ChangeSourceType,
                Comment = request.Comment?.Trim(),
                ChangedAt = service.LastStatusChangedAt
            };

            history.Add(item);
            return item;
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
