using DeveloperRegistry.Api.Features.Applications.UpdateApplication;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class UpdateApplicationValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenIdInvalid()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("bad", "Name", "Desc"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var validator = new Validator();
        var result = await validator.ValidateAsync(new Command("00000000000000000000000027", "Name", "Desc"));
        result.IsValid.Should().BeTrue();
    }
}
