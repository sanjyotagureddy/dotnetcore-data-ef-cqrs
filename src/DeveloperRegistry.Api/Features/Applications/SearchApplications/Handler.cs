using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.Applications.SearchApplications;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyList<ApplicationSearchItem>> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select id, name, description, is_archived as IsArchived, updated_at_utc as UpdatedAtUtc
            from applications
            where (@Search is null or @Search = '' or name ilike '%' || @Search || '%')
            order by updated_at_utc desc
            limit 200;
            """;

        var command = new CommandDefinition(sql, new { query.Search }, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<ApplicationSearchItem>(command);
        return rows.ToList();
    }
}
