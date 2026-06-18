using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Features.Webhooks.EnableWebhook;
using DeveloperRegistry.UnitTests.Support;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Webhooks;

public sealed class EnableWebhookHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldEnableWebhook()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var app = RegisteredApplication.Create("00000000000000000000000021", "Portal", "Desc", DateTime.UtcNow);
        var webhook = Webhook.Create("00000000000000000000000022", app.Id, "event", "https://hooks.example.com/e", DateTime.UtcNow);
        webhook.Disable();
        dbContext.Applications.Add(app);
        dbContext.Webhooks.Add(webhook);
        await dbContext.SaveChangesAsync();

        var handler = new Handler(dbContext, new Validator());

        var response = await handler.HandleAsync(new Command(webhook.Id), CancellationToken.None);

        response.Enabled.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenWebhookMissing()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new Handler(dbContext, new Validator());

        var act = () => handler.HandleAsync(new Command("00000000000000000000000023"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
