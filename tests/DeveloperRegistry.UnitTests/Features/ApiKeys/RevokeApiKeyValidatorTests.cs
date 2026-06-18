using DeveloperRegistry.Api.Features.ApiKeys.RevokeApiKey;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class RevokeApiKeyValidatorTests
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
        var result = await validator.ValidateAsync(new Command("00000000000000000000000033"));
        result.IsValid.Should().BeTrue();
    }
}
