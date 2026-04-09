using FirstAppForBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstAppForBank.Infrastructure.Persistence.Configurations;

public sealed class ServiceStatusHistoryConfiguration : IEntityTypeConfiguration<ServiceStatusHistory>
{
    public void Configure(EntityTypeBuilder<ServiceStatusHistory> builder)
    {
        builder.ToTable("service_status_history");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OldStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.NewStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ChangeSource)
            .HasMaxLength(200);

        builder.Property(x => x.ChangeSourceType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(2000);

        builder.Property(x => x.ChangedAt)
            .HasColumnName("changed_at")
            .IsRequired();

        builder.HasOne(x => x.Service)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
