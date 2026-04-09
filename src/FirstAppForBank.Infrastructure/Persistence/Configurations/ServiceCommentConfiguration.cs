using FirstAppForBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstAppForBank.Infrastructure.Persistence.Configurations;

public sealed class ServiceCommentConfiguration : IEntityTypeConfiguration<ServiceComment>
{
    public void Configure(EntityTypeBuilder<ServiceComment> builder)
    {
        builder.ToTable("service_comments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AuthorName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CommentText)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(x => x.Service)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
