using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperRegistry.Api.Persistence.Configurations;

public sealed class WebhookConfiguration : IEntityTypeConfiguration<Webhook>
{
    public void Configure(EntityTypeBuilder<Webhook> builder)
    {
        builder.ToTable("webhooks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.ApplicationId).HasColumnName("application_id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.EventName).HasColumnName("event_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Url).HasColumnName("url").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Enabled).HasColumnName("enabled").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();

        builder.HasIndex(x => new { x.ApplicationId, x.EventName });
    }
}
