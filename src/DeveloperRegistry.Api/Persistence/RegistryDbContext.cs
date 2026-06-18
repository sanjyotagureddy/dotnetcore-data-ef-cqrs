using DeveloperRegistry.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Persistence;

public sealed class RegistryDbContext(DbContextOptions<RegistryDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredApplication> Applications => Set<RegisteredApplication>();

    public DbSet<Owner> Owners => Set<Owner>();

    public DbSet<ApplicationOwner> ApplicationOwners => Set<ApplicationOwner>();

    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    public DbSet<Webhook> Webhooks => Set<Webhook>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegistryDbContext).Assembly);
    }
}
