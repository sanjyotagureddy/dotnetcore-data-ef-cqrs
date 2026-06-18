using DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class RotateApiKeyValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenIdInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("bad", null));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000034", null));
        result.IsValid.Should().BeTrue();
    }
}
