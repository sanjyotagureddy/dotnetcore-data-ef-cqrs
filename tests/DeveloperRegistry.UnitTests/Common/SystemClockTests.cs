using DeveloperRegistry.Api.Common.Time;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Common;

public sealed class SystemClockTests
{
    [Fact]
    public void UtcNow_ShouldBeCloseToCurrentUtcTime()
    {
        var sut = new SystemClock();
        var before = DateTime.UtcNow;

        var value = sut.UtcNow;

        var after = DateTime.UtcNow;
        value.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }
}
