using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyList<Response>> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select id, name, status, created_at_utc as CreatedAtUtc, expires_at_utc as ExpiresAtUtc
            from api_keys
            where application_id = @ApplicationId
            order by created_at_utc desc;
            """;

        var command = new CommandDefinition(sql, new { query.ApplicationId }, cancellationToken: cancellationToken);
        return (await connection.QueryAsync<Response>(command)).ToList();
    }
}
