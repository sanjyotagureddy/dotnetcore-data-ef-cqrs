using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperRegistry.Api.Persistence.Configurations;

public sealed class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable("api_keys");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.ApplicationId).HasColumnName("application_id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.KeyHash).HasColumnName("key_hash").HasMaxLength(64).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.ExpiresAtUtc).HasColumnName("expires_at_utc");

        builder.HasIndex(x => x.KeyHash).IsUnique();
        builder.HasIndex(x => new { x.ApplicationId, x.Status });
    }
}
