using DeveloperRegistry.Api.Features.Applications.CreateApplication;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Features.Applications;

public sealed class CreateApplicationValidatorTests
{
    private readonly Validator _validator = new();

    [Fact]
    public async Task Validate_ShouldFail_WhenNameMissing()
    {
        var result = await _validator.ValidateAsync(new Command(string.Empty, "desc"));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldPass_ForValidRequest()
    {
        var result = await _validator.ValidateAsync(new Command("Dev Portal", "desc"));
        result.IsValid.Should().BeTrue();
    }
}
