using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class DbServiceCatalogReader(AppDbContext dbContext) : IServiceCatalogReader
{
    public async Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Services
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Category!.Name)
            .ThenBy(x => x.Name)
            .Select(x => new ServiceCardDto
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category != null ? x.Category.Name : "Uncategorized",
                Status = x.CurrentStatus.ToString(),
                Criticality = x.Criticality.ToString(),
                ServiceType = x.ServiceType.ToString(),
                Owner = x.Owner,
                LastStatusChangedAt = x.LastStatusChangedAt
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ServiceDetailsDto?> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Services
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.StatusHistory)
            .Include(x => x.Comments)
            .Where(x => x.Id == serviceId)
            .Select(x => new ServiceDetailsDto
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category != null ? x.Category.Name : "Uncategorized",
                Description = x.Description ?? string.Empty,
                Status = x.CurrentStatus.ToString(),
                Criticality = x.Criticality.ToString(),
                ServiceType = x.ServiceType.ToString(),
                Owner = x.Owner,
                RunbookUrl = x.RunbookUrl,
                DashboardUrl = x.DashboardUrl,
                Notes = x.Notes,
                LastStatusChangedAt = x.LastStatusChangedAt,
                StatusHistory = x.StatusHistory
                    .OrderByDescending(h => h.ChangedAt)
                    .Select(h => new ServiceStatusHistoryItemDto
                    {
                        OldStatus = h.OldStatus.HasValue ? h.OldStatus.Value.ToString() : null,
                        NewStatus = h.NewStatus.ToString(),
                        ChangeSource = h.ChangeSource,
                        ChangeSourceType = h.ChangeSourceType,
                        Comment = h.Comment,
                        ChangedAt = h.ChangedAt
                    })
                    .ToArray(),
                Comments = x.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new ServiceCommentDto
                    {
                        AuthorName = c.AuthorName,
                        CommentText = c.CommentText,
                        CreatedAt = c.CreatedAt
                    })
                    .ToArray()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
