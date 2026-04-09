namespace FirstAppForBank.Application.Services;

public interface IAdministrationCatalogService
{
    Task<IReadOnlyCollection<CategoryAdminDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ServiceAdminDto>> GetServicesAsync(CancellationToken cancellationToken = default);

    Task<CategoryAdminDto> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken cancellationToken = default);

    Task<CategoryAdminDto?> UpdateCategoryAsync(Guid categoryId, UpsertCategoryRequest request, CancellationToken cancellationToken = default);

    Task<ServiceAdminDto> CreateServiceAsync(UpsertServiceRequest request, CancellationToken cancellationToken = default);

    Task<ServiceAdminDto?> UpdateServiceAsync(Guid serviceId, UpsertServiceRequest request, CancellationToken cancellationToken = default);
}
