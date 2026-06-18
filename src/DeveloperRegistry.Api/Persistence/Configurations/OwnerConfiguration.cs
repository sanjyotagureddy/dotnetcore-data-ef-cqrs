using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperRegistry.Api.Persistence.Configurations;

public sealed class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("owners");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasMaxLength(26).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.ApplicationOwners)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
