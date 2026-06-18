using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Owners.AddOwner;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Owners;

public sealed class AddOwnerHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateOwnerAndRelationship()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000005", "Portal", "Desc", DateTime.UtcNow.AddDays(-1));
        dbContext.Applications.Add(app);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new FakeClock(new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc)), new Validator());

        var response = await handler.HandleAsync(new Command(app.Id, "Alice", "Alice@Example.com"), CancellationToken.None);

        response.Email.Should().Be("alice@example.com");
        dbContext.Owners.Should().ContainSingle();
        dbContext.ApplicationOwners.Should().ContainSingle(x => x.ApplicationId == app.Id && x.OwnerId == response.OwnerId);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenApplicationMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new FakeClock(DateTime.UtcNow), new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000006", "Bob", "bob@example.com"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
