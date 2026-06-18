using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Applications.UpdateApplication;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class UpdateApplicationHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldUpdateApplication()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var now = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc);
        var existing = RegisteredApplication.Create("00000000000000000000000001", "Old", "Old desc", now.AddDays(-1));
        dbContext.Applications.Add(existing);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new FakeClock(now), new Validator());

        var response = await handler.HandleAsync(new Command(existing.Id, "New Name", "New desc"), CancellationToken.None);

        response.Name.Should().Be("New Name");
        response.Description.Should().Be("New desc");
        response.UpdatedAtUtc.Should().Be(now);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenNotFound()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new FakeClock(DateTime.UtcNow), new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000002", "Name", "Desc"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
