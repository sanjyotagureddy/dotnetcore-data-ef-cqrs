using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Owners.RemoveOwner;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Owners;

public sealed class RemoveOwnerHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldRemoveRelation_WhenFound()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000007", "Portal", "Desc", DateTime.UtcNow);
        var owner = Owner.Create("00000000000000000000000008", "Owner", "owner@example.com", DateTime.UtcNow);
        dbContext.Applications.Add(app);
        dbContext.Owners.Add(owner);
        dbContext.ApplicationOwners.Add(ApplicationOwner.Create(app.Id, owner.Id, DateTime.UtcNow));
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new Validator());

        var response = await handler.HandleAsync(new Command(app.Id, owner.Id), CancellationToken.None);

        response.Removed.Should().BeTrue();
        dbContext.ApplicationOwners.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFalse_WhenRelationMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new Validator());

        var response = await handler.HandleAsync(new Command("00000000000000000000000009", "00000000000000000000000010"), CancellationToken.None);

        response.Removed.Should().BeFalse();
    }
}
