using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure.Options;
using FirstAppForBank.Infrastructure.Persistence;
using FirstAppForBank.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FirstAppForBank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var usePostgres = configuration.GetValue<bool>("Infrastructure:UsePostgres");
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.Configure<PrometheusOptions>(configuration.GetSection("Prometheus"));
        services.AddHttpClient<PrometheusMetricHistoryReader>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<PrometheusOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, options.TimeoutSeconds));
        });

        var usePrometheus = configuration.GetValue<bool>("Prometheus:Enabled") && !string.IsNullOrWhiteSpace(configuration["Prometheus:BaseUrl"]);
        if (usePrometheus)
        {
            services.AddScoped<IMetricHistoryReader, PrometheusMetricHistoryReader>();
        }
        else
        {
            services.AddSingleton<IMetricHistoryReader, InMemoryMetricHistoryReader>();
        }

        if (!usePostgres || string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddSingleton<IServiceCatalogReader, InMemoryServiceCatalogReader>();
            return services;
        }

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IServiceCatalogReader, DbServiceCatalogReader>();
        services.AddScoped<AppDbContextSeed>();

        return services;
    }

    public static async Task InitializeInfrastructureAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        if (dbContext is null)
        {
            return;
        }

        await dbContext.Database.MigrateAsync(cancellationToken);

        var seed = scope.ServiceProvider.GetRequiredService<AppDbContextSeed>();
        await seed.SeedAsync(cancellationToken);
    }
}
