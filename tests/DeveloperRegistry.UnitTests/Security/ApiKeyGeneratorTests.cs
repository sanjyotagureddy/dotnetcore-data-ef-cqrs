using DeveloperRegistry.Api.Common.Security;
using FluentAssertions;

namespace DeveloperRegistry.UnitTests.Security;

public sealed class ApiKeyGeneratorTests
{
    [Fact]
    public void Hash_ShouldProduceStableLength()
    {
        var hash = ApiKeyGenerator.Hash("abc123");
        hash.Should().HaveLength(64);
    }
}
