using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperRegistry.Api.Persistence.Configurations;

public sealed class ApplicationOwnerConfiguration : IEntityTypeConfiguration<ApplicationOwner>
{
    public void Configure(EntityTypeBuilder<ApplicationOwner> builder)
    {
        builder.ToTable("application_owners");

        builder.HasKey(x => new { x.ApplicationId, x.OwnerId });
        builder.Property(x => x.ApplicationId).HasColumnName("application_id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.AddedAtUtc).HasColumnName("added_at_utc").IsRequired();

        builder.HasIndex(x => x.OwnerId);
    }
}
