using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeveloperRegistry.Api.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RegistryDbContext>
{
    public RegistryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RegistryDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=developer_registry;Username=postgres;Password=postgres");
        return new RegistryDbContext(optionsBuilder.Options);
    }
}
