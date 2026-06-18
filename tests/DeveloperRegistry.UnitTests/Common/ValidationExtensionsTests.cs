using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Features.Applications.CreateApplication;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Common;

public sealed class ValidationExtensionsTests
{
    [Fact]
    public async Task ValidateRequestAsync_ShouldThrow_WhenInvalid()
    {
        var validator = new Validator();
        var act = () => validator.ValidateRequestAsync(new Command(string.Empty, "desc"), CancellationToken.None);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task ValidateRequestAsync_ShouldNotThrow_WhenValid()
    {
        var validator = new Validator();
        var act = () => validator.ValidateRequestAsync(new Command("Portal", "desc"), CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
}
