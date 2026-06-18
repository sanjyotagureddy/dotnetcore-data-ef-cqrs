using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class CreateApiKeyHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateApiKey()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000011", "Portal", "Desc", DateTime.UtcNow);
        dbContext.Applications.Add(app);
        await dbContext.SaveChangesAsync();

        var now = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc);
        var handler = new Handler(dbContext, new FakeClock(now), new Validator());

        var response = await handler.HandleAsync(new Command(app.Id, "Primary", now.AddDays(30)), CancellationToken.None);

        response.PlainTextKey.Should().NotBeNullOrWhiteSpace();
        dbContext.ApiKeys.Should().ContainSingle(x => x.ApplicationId == app.Id && x.Status == ApiKeyStatus.Active);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenApplicationMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new FakeClock(DateTime.UtcNow), new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000012", "Primary", null), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
