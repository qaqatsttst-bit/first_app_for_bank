using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure;
using FirstAppForBank.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeInfrastructureAsync();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    application = "Manage MegaPayment Service"
}));

app.MapGet("/api/services", async (IServiceCatalogReader reader, CancellationToken cancellationToken) =>
{
    var services = await reader.GetServicesAsync(cancellationToken);
    return Results.Ok(services);
});

app.MapGet("/api/services/{id:guid}", async (Guid id, IServiceCatalogReader reader, CancellationToken cancellationToken) =>
{
    var service = await reader.GetServiceByIdAsync(id, cancellationToken);
    return service is null ? Results.NotFound() : Results.Ok(service);
});

app.MapGet("/api/services/{id:guid}/metrics", async (Guid id, string? range, IServiceCatalogReader catalogReader, IMetricHistoryReader metricReader, CancellationToken cancellationToken) =>
{
    var service = await catalogReader.GetServiceByIdAsync(id, cancellationToken);
    if (service is null)
    {
        return Results.NotFound();
    }

    var metricsRange = ParseRange(range);
    var metrics = await metricReader.GetMetricsAsync(service.Name, metricsRange, cancellationToken);
    return Results.Ok(metrics);
});

app.Run();

return;

static MetricsRange ParseRange(string? range)
{
    return range?.ToLowerInvariant() switch
    {
        "7d" => MetricsRange.Last7Days,
        "30d" => MetricsRange.Last30Days,
        _ => MetricsRange.Last24Hours
    };
}

public partial class Program;
