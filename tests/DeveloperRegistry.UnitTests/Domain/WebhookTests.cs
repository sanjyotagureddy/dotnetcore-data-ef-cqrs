using DeveloperRegistry.Api.Domain;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Domain;

public sealed class WebhookTests
{
    [Fact]
    public void Disable_ThenEnable_ShouldToggleState()
    {
        var webhook = Webhook.Create("00000000000000000000000041", "00000000000000000000000042", "evt", "https://hooks.example.com", DateTime.UtcNow);

        webhook.Disable();
        webhook.Enabled.Should().BeFalse();

        webhook.Enable();
        webhook.Enabled.Should().BeTrue();
    }
}
