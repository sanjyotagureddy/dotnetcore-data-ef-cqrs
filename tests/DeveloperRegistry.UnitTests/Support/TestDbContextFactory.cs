using DeveloperRegistry.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.UnitTests.Support;

internal static class TestDbContextFactory
{
    public static RegistryDbContext Create()
    {
        var options = new DbContextOptionsBuilder<RegistryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        return new RegistryDbContext(options);
    }
}
