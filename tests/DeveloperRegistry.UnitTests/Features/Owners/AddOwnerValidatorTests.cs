using DeveloperRegistry.Api.Features.Owners.AddOwner;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Owners;

public sealed class AddOwnerValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenEmailInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000029", "Alice", "bad-email"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000029", "Alice", "alice@example.com"));
        result.IsValid.Should().BeTrue();
    }
}
