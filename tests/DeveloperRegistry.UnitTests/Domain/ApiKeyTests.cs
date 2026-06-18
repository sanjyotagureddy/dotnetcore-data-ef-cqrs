using DeveloperRegistry.Api.Domain;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Domain;

public sealed class ApiKeyTests
{
    [Fact]
    public void Rotate_ShouldUpdateHashAndExpiry()
    {
        var key = ApiKey.Create("00000000000000000000000039", "00000000000000000000000040", "Primary", "oldhash", DateTime.UtcNow, null);

        var expires = DateTime.UtcNow.AddDays(7);
        key.Rotate("newhash", expires);

        key.Status.Should().Be(ApiKeyStatus.Active);
        key.KeyHash.Should().Be("newhash");
        key.ExpiresAtUtc.Should().Be(expires);
    }

    [Fact]
    public void Rotate_ShouldThrow_WhenRevoked()
    {
        var key = ApiKey.Create("00000000000000000000000039", "00000000000000000000000040", "Primary", "oldhash", DateTime.UtcNow, null);
        key.Revoke();

        var act = () => key.Rotate("newhash", DateTime.UtcNow.AddDays(7));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Revoke_ShouldThrow_WhenAlreadyRevoked()
    {
        var key = ApiKey.Create("00000000000000000000000039", "00000000000000000000000040", "Primary", "oldhash", DateTime.UtcNow, null);
        key.Revoke();

        var act = () => key.Revoke();

        act.Should().Throw<InvalidOperationException>();
    }
}
