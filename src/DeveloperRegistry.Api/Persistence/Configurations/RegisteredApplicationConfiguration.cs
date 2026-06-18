using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperRegistry.Api.Persistence.Configurations;

public sealed class RegisteredApplicationConfiguration : IEntityTypeConfiguration<RegisteredApplication>
{
    public void Configure(EntityTypeBuilder<RegisteredApplication> builder)
    {
        builder.ToTable("applications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000).IsRequired();
        builder.Property(x => x.IsArchived).HasColumnName("is_archived").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc").IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.IsArchived);

        builder.HasMany(x => x.ApiKeys)
            .WithOne(x => x.Application)
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Webhooks)
            .WithOne(x => x.Application)
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ApplicationOwners)
            .WithOne(x => x.Application)
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
