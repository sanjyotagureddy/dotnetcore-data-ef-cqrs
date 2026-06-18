using DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.ApiKeys;

public sealed class CreateApiKeyValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenNameMissing()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000032", string.Empty, null));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000032", "Primary", null));
        result.IsValid.Should().BeTrue();
    }
}
