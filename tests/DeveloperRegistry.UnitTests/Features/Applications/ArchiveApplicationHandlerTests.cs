using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Applications.ArchiveApplication;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class ArchiveApplicationHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldArchiveApplication()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var now = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc);
        var app = RegisteredApplication.Create("00000000000000000000000003", "Portal", "Desc", now.AddDays(-2));
        dbContext.Applications.Add(app);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new FakeClock(now), new Validator());

        var response = await handler.HandleAsync(new Command(app.Id), CancellationToken.None);

        response.IsArchived.Should().BeTrue();
        response.UpdatedAtUtc.Should().Be(now);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenNotFound()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new FakeClock(DateTime.UtcNow), new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000004"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
