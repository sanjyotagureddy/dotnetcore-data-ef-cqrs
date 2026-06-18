using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.ApiKeys.RevokeApiKey;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class RevokeApiKeyHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldRevokeApiKey()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000013", "Portal", "Desc", DateTime.UtcNow);
        var apiKey = ApiKey.Create("00000000000000000000000014", app.Id, "Primary", "hash", DateTime.UtcNow, null);
        dbContext.Applications.Add(app);
        dbContext.ApiKeys.Add(apiKey);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new Validator());

        var response = await handler.HandleAsync(new Command(apiKey.Id), CancellationToken.None);

        response.Status.Should().Be("Revoked");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenApiKeyMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000015"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
