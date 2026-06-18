using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Webhooks;

public sealed class RegisterWebhookHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldRegisterWebhook()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000019", "Portal", "Desc", DateTime.UtcNow);
        dbContext.Applications.Add(app);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new FakeClock(new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc)), new Validator());

        var response = await handler.HandleAsync(new Command(app.Id, "deployment.completed", "https://hooks.example.com/deploy"), CancellationToken.None);

        response.Enabled.Should().BeTrue();
        dbContext.Webhooks.Should().ContainSingle(x => x.ApplicationId == app.Id);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenApplicationMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new FakeClock(DateTime.UtcNow), new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000020", "event", "https://hooks.example.com/x"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
