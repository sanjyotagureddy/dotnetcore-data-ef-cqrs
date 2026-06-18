using DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Webhooks;

public sealed class RegisterWebhookValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenUrlInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000035", "evt", "bad-url"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000035", "evt", "https://hooks.example.com/x"));
        result.IsValid.Should().BeTrue();
    }
}
