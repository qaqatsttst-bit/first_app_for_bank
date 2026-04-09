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

app.Run();

public partial class Program;
