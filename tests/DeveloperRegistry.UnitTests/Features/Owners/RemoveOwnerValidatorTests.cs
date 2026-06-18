using DeveloperRegistry.Api.Features.Owners.RemoveOwner;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Owners;

public sealed class RemoveOwnerValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenOwnerIdInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000030", "bad"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000030", "00000000000000000000000031"));
        result.IsValid.Should().BeTrue();
    }
}
