using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyList<Response>> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select id, event_name as EventName, url, enabled
            from webhooks
            where application_id = @ApplicationId
            order by event_name;
            """;

        var command = new CommandDefinition(sql, new { query.ApplicationId }, cancellationToken: cancellationToken);
        return (await connection.QueryAsync<Response>(command)).ToList();
    }
}
