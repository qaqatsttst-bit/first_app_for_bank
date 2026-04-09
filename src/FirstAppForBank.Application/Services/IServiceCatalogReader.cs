namespace FirstAppForBank.Application.Services;

public interface IServiceCatalogReader
{
    Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default);

    Task<ServiceDetailsDto?> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken = default);
}
