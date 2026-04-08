using FirstAppForBank.Application.Services;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryServiceCatalogReader : IServiceCatalogReader
{
    public Task<IReadOnlyCollection<ServiceCardDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<ServiceCardDto> services =
        [
            new(
                Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"),
                "KoronaPay",
                "External Payments",
                "Ok",
                "Critical",
                "Payments Team"),
            new(
                Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222"),
                "Payment Routing",
                "Internal Payment Core",
                "Degraded",
                "High",
                "Core Banking Team")
        ];

        return Task.FromResult(services);
    }
}
