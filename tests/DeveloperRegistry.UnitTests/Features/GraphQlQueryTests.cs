using System.Data;
using Dapper;
using DeveloperRegistry.Api.Features.GraphQL;
using DeveloperRegistry.Api.Persistence;
using FluentAssertions;
using Moq;
using Moq.Dapper;

namespace DeveloperRegistry.UnitTests.Features;

public sealed class GraphQlQueryTests
{
    [Fact]
    public async Task ApplicationsAsync_ShouldReturnSearchResults()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        IReadOnlyList<DeveloperRegistry.Api.Features.Applications.SearchApplications.ApplicationSearchItem> rows =
        [
            new("app1", "Portal", "Main portal", false, DateTime.UtcNow),
        ];

        connection
             .SetupDapperAsync(c => c.QueryAsync<DeveloperRegistry.Api.Features.Applications.SearchApplications.ApplicationSearchItem>(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(rows);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var query = new Query();
        var handler = new DeveloperRegistry.Api.Features.Applications.SearchApplications.Handler(factory.Object);

        var result = await query.ApplicationsAsync("port", handler, CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Name.Should().Be("Portal");
    }

    [Fact]
    public async Task ApplicationDashboardAsync_ShouldReturnDashboard()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        var dashboard = new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.ApplicationDashboard(
            "app1",
            "Portal",
            false,
            2,
            1,
            3,
            2,
            DateTime.UtcNow);

        connection
             .SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.ApplicationDashboard>(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(dashboard);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var query = new Query();
        var handler = new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.Handler(factory.Object);

        var result = await query.ApplicationDashboardAsync("app1", handler, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be("app1");
    }
}
