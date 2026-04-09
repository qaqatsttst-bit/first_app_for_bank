using FirstAppForBank.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAppForBank.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<ServiceStatusHistory> ServiceStatusHistory => Set<ServiceStatusHistory>();

    public DbSet<ServiceComment> ServiceComments => Set<ServiceComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
