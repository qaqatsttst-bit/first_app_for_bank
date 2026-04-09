using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class DbAdministrationCatalogService(AppDbContext dbContext) : IAdministrationCatalogService
{
    public async Task<IReadOnlyCollection<CategoryAdminDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new CategoryAdminDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ServiceAdminDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Services
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .Select(x => new ServiceAdminDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : "Uncategorized",
                Description = x.Description,
                Criticality = x.Criticality,
                ServiceType = x.ServiceType,
                Status = x.CurrentStatus,
                Owner = x.Owner,
                IsActive = x.IsActive
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<CategoryAdminDto> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Domain.Models.Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsActive = request.IsActive
        };

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CategoryAdminDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryAdminDto?> UpdateCategoryAsync(Guid categoryId, UpsertCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);
        if (category is null)
        {
            return null;
        }

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim();
        category.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CategoryAdminDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }

    public async Task<ServiceAdminDto> CreateServiceAsync(UpsertServiceRequest request, CancellationToken cancellationToken = default)
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

        dbContext.Services.Add(service);
        await dbContext.SaveChangesAsync(cancellationToken);

        var categoryName = await dbContext.Categories.Where(x => x.Id == request.CategoryId).Select(x => x.Name).FirstOrDefaultAsync(cancellationToken) ?? "Uncategorized";

        return new ServiceAdminDto
        {
            Id = service.Id,
            Name = service.Name,
            CategoryId = service.CategoryId,
            CategoryName = categoryName,
            Description = service.Description,
            Criticality = service.Criticality,
            ServiceType = service.ServiceType,
            Status = service.CurrentStatus,
            Owner = service.Owner,
            IsActive = service.IsActive
        };
    }

    public async Task<ServiceAdminDto?> UpdateServiceAsync(Guid serviceId, UpsertServiceRequest request, CancellationToken cancellationToken = default)
    {
        var service = await dbContext.Services.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == serviceId, cancellationToken);
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

        await dbContext.SaveChangesAsync(cancellationToken);

        var categoryName = await dbContext.Categories.Where(x => x.Id == request.CategoryId).Select(x => x.Name).FirstOrDefaultAsync(cancellationToken) ?? "Uncategorized";

        return new ServiceAdminDto
        {
            Id = service.Id,
            Name = service.Name,
            CategoryId = service.CategoryId,
            CategoryName = categoryName,
            Description = service.Description,
            Criticality = service.Criticality,
            ServiceType = service.ServiceType,
            Status = service.CurrentStatus,
            Owner = service.Owner,
            IsActive = service.IsActive
        };
    }
}
