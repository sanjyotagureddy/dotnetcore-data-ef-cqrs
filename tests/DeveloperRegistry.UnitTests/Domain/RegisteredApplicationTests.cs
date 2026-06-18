using DeveloperRegistry.Api.Domain;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Domain;

public sealed class RegisteredApplicationTests
{
    [Fact]
    public void Archive_ShouldSetFlagAndUpdateTimestamp()
    {
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt = createdAt.AddDays(1);
        var app = RegisteredApplication.Create("01JTESTAPP0000000000000000", "Portal", "Portal app", createdAt);

        app.Archive(updatedAt);

        app.IsArchived.Should().BeTrue();
        app.UpdatedAtUtc.Should().Be(updatedAt);
    }
}
