using DeveloperRegistry.Api.Features.Applications.ArchiveApplication;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class ArchiveApplicationValidatorTests
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
        var result = await validator.ValidateAsync(new Command("00000000000000000000000028"));
        result.IsValid.Should().BeTrue();
    }
}
