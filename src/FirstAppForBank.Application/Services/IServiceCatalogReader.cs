namespace FirstAppForBank.Application.Services;

public interface IServiceCatalogReader
{
    Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default);
}
