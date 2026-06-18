using DeveloperRegistry.Api.Domain;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Domain;

public sealed class OwnerTests
{
    [Fact]
    public void Create_ShouldNormalizeEmail()
    {
        var owner = Owner.Create("00000000000000000000000038", "Alice", "Alice@Example.com", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        owner.Email.Should().Be("alice@example.com");
        owner.Name.Should().Be("Alice");
    }
}
