using System.Data;
using Dapper;
using DeveloperRegistry.Api.Persistence;
using FluentAssertions;
using Moq;
using Moq.Dapper;

namespace DeveloperRegistry.UnitTests.Features;

public sealed class ReadModelHandlersTests
{
    [Fact]
    public async Task SearchApplications_Handler_ShouldReturnRows()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        var now = DateTime.UtcNow;
        IReadOnlyList<DeveloperRegistry.Api.Features.Applications.SearchApplications.ApplicationSearchItem> rows =
        [
            new("app1", "Portal", "Main portal", false, now),
            new("app2", "Billing", "Billing app", true, now.AddMinutes(-5)),
        ];

        connection
             .SetupDapperAsync(c => c.QueryAsync<DeveloperRegistry.Api.Features.Applications.SearchApplications.ApplicationSearchItem>(It.IsAny<Dapper.CommandDefinition>()))
            .ReturnsAsync(rows);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var sut = new DeveloperRegistry.Api.Features.Applications.SearchApplications.Handler(factory.Object);

        var result = await sut.HandleAsync(new DeveloperRegistry.Api.Features.Applications.SearchApplications.Query("port"), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Id.Should().Be("app1");
    }

    [Fact]
    public async Task GetApplicationDashboard_Handler_ShouldReturnDashboard()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        var now = DateTime.UtcNow;
        var row = new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.ApplicationDashboard(
            "app1",
            "Portal",
            false,
            2,
            3,
            4,
            3,
            now);

        connection
             .SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.ApplicationDashboard>(It.IsAny<Dapper.CommandDefinition>()))
            .ReturnsAsync(row);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var sut = new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.Handler(factory.Object);

        var result = await sut.HandleAsync(new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.Query("app1"), CancellationToken.None);

        result.Should().NotBeNull();
        result!.OwnerCount.Should().Be(2);
        result.ActiveApiKeyCount.Should().Be(3);
    }

    [Fact]
    public async Task GetApplicationApiKeys_Handler_ShouldReturnRows()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        var now = DateTime.UtcNow;
        IReadOnlyList<DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys.Response> rows =
        [
            new("key1", "Primary", "Active", now, now.AddDays(30)),
            new("key2", "Backup", "Revoked", now.AddDays(-1), null),
        ];

        connection
            .SetupDapperAsync(c => c.QueryAsync<DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys.Response>(It.IsAny<Dapper.CommandDefinition>()))
            .ReturnsAsync(rows);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var sut = new DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys.Handler(factory.Object);

        var result = await sut.HandleAsync(new DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys.Query("app1"), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Status.Should().Be("Active");
    }

    [Fact]
    public async Task GetApplicationWebhooks_Handler_ShouldReturnRows()
    {
        var connection = new Mock<IDbConnection>();
        var factory = new Mock<ISqlConnectionFactory>();
        IReadOnlyList<DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks.Response> rows =
        [
            new("wh1", "app.created", "https://example.test/hooks/created", true),
            new("wh2", "app.updated", "https://example.test/hooks/updated", false),
        ];

        connection
            .SetupDapperAsync(c => c.QueryAsync<DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks.Response>(It.IsAny<Dapper.CommandDefinition>()))
            .ReturnsAsync(rows);
        factory
            .Setup(x => x.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(connection.Object);

        var sut = new DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks.Handler(factory.Object);

        var result = await sut.HandleAsync(new DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks.Query("app1"), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].EventName.Should().Be("app.created");
    }
}
