using DeveloperRegistry.Api.Features.Webhooks.DisableWebhook;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Webhooks;

public sealed class DisableWebhookValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenIdInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("bad"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000037"));
        result.IsValid.Should().BeTrue();
    }
}
