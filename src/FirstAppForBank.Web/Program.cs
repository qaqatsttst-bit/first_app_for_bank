using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IServiceCatalogReader, InMemoryServiceCatalogReader>();

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/health"));

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    application = "FirstAppForBank.Web"
}));

app.MapGet("/api/services", async (IServiceCatalogReader reader, CancellationToken cancellationToken) =>
{
    var services = await reader.GetServicesAsync(cancellationToken);
    return Results.Ok(services);
});

app.Run();

public partial class Program;
