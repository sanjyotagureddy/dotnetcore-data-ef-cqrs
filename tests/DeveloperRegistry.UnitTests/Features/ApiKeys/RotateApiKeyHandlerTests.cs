using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class RotateApiKeyHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldRotateApiKey_AndActivateIt()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000016", "Portal", "Desc", DateTime.UtcNow);
        var apiKey = ApiKey.Create("00000000000000000000000017", app.Id, "Primary", "old-hash", DateTime.UtcNow, null);
        apiKey.Revoke();
        dbContext.Applications.Add(app);
        dbContext.ApiKeys.Add(apiKey);
        await dbContext.SaveChangesAsync();

        var expiry = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc);
        var handler = new Handler(dbContext, new Validator());

        var response = await handler.HandleAsync(new Command(apiKey.Id, expiry), CancellationToken.None);

        response.Status.Should().Be("Active");
        response.ExpiresAtUtc.Should().Be(expiry);
        response.PlainTextKey.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenApiKeyMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000018", null), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
