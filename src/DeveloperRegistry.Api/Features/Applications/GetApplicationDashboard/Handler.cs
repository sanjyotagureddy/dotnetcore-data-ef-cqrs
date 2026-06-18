using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<ApplicationDashboard?> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select
                a.id,
                a.name,
                a.is_archived as IsArchived,
                (select count(*) from application_owners ao where ao.application_id = a.id) as OwnerCount,
                (select count(*) from api_keys ak where ak.application_id = a.id and ak.status = 'Active') as ActiveApiKeyCount,
                (select count(*) from webhooks w where w.application_id = a.id) as WebhookCount,
                (select count(*) from webhooks w where w.application_id = a.id and w.enabled = true) as EnabledWebhookCount,
                a.updated_at_utc as UpdatedAtUtc
            from applications a
            where a.id = @Id;
            """;

        var command = new CommandDefinition(sql, new { query.Id }, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<ApplicationDashboard>(command);
    }
}
