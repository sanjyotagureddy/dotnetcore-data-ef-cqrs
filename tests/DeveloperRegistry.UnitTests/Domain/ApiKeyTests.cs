using DeveloperRegistry.Api.Domain;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Domain;

public sealed class ApiKeyTests
{
    [Fact]
    public void Rotate_ShouldSetActiveAndUpdateValues()
    {
        var key = ApiKey.Create("00000000000000000000000039", "00000000000000000000000040", "Primary", "oldhash", DateTime.UtcNow, null);
        key.Revoke();

        var expires = DateTime.UtcNow.AddDays(7);
        key.Rotate("newhash", expires);

        key.Status.Should().Be(ApiKeyStatus.Active);
        key.KeyHash.Should().Be("newhash");
        key.ExpiresAtUtc.Should().Be(expires);
    }
}
