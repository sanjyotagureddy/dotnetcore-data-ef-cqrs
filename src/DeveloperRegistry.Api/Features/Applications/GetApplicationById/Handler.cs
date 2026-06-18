using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.Applications.GetApplicationById;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<ApplicationDetails?> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select id, name, description, is_archived as IsArchived, created_at_utc as CreatedAtUtc, updated_at_utc as UpdatedAtUtc
            from applications
            where id = @Id;

            select o.id, o.name, o.email
            from owners o
            inner join application_owners ao on ao.owner_id = o.id
            where ao.application_id = @Id
            order by o.name;

            select id, name, status, created_at_utc as CreatedAtUtc, expires_at_utc as ExpiresAtUtc
            from api_keys
            where application_id = @Id
            order by created_at_utc desc;

            select id, event_name as EventName, url, enabled
            from webhooks
            where application_id = @Id
            order by event_name;
            """;

        var command = new CommandDefinition(sql, new { query.Id }, cancellationToken: cancellationToken);
        using var grid = await connection.QueryMultipleAsync(command);

        var app = await grid.ReadSingleOrDefaultAsync<ApplicationRow>();
        if (app is null)
        {
            return null;
        }

        var owners = (await grid.ReadAsync<OwnerItem>()).ToList();
        var apiKeys = (await grid.ReadAsync<ApiKeyItem>()).ToList();
        var webhooks = (await grid.ReadAsync<WebhookItem>()).ToList();

        return new ApplicationDetails(app.Id, app.Name, app.Description, app.IsArchived, app.CreatedAtUtc, app.UpdatedAtUtc, owners, apiKeys, webhooks);
    }

    private sealed record ApplicationRow(string Id, string Name, string Description, bool IsArchived, DateTime CreatedAtUtc, DateTime UpdatedAtUtc);
}
