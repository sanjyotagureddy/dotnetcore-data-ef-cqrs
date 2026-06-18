using DeveloperRegistry.Api.Features.Applications.CreateApplication;
using DeveloperRegistry.Api.Persistence;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class CreateApplicationHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldPersistApplication()
    {
        await using var dbContext = CreateDbContext();
        var handler = new Handler(dbContext, new FakeClock(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)), new Validator());

        var response = await handler.HandleAsync(new Command("Portal", "Developer Portal"), CancellationToken.None);

        response.Id.Should().NotBeNullOrWhiteSpace();
        dbContext.Applications.Should().ContainSingle(x => x.Id == response.Id);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenNameIsDuplicate()
    {
        await using var dbContext = CreateDbContext();
        var validator = new Validator();
        var handler = new Handler(dbContext, new FakeClock(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)), validator);

        await handler.HandleAsync(new Command("Portal", "Developer Portal"), CancellationToken.None);

        var act = () => handler.HandleAsync(new Command("Portal", "Duplicate"), CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static RegistryDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<RegistryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new RegistryDbContext(options);
    }
}
