using FirstAppForBank.Application.Services;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
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

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var seed = scope.ServiceProvider.GetRequiredService<AppDbContextSeed>();
        await seed.SeedAsync(cancellationToken);
    }
}
