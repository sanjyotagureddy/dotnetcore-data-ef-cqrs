using Dapper;
using DeveloperRegistry.Api.Persistence;

namespace DeveloperRegistry.Api.Features.Owners.GetOwnerApplications;

public sealed class Handler(ISqlConnectionFactory connectionFactory)
{
    public async Task<OwnerDetails?> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql = """
            select id, name, email
            from owners
            where id = @OwnerId;

            select a.id, a.name, a.description, a.is_archived as IsArchived
            from applications a
            inner join application_owners ao on ao.application_id = a.id
            where ao.owner_id = @OwnerId
            order by a.name;
            """;

        var command = new CommandDefinition(sql, new { query.OwnerId }, cancellationToken: cancellationToken);
        using var grid = await connection.QueryMultipleAsync(command);

        var owner = await grid.ReadSingleOrDefaultAsync<OwnerRow>();
        if (owner is null)
        {
            return null;
        }

        var apps = (await grid.ReadAsync<ApplicationItem>()).ToList();
        return new OwnerDetails(owner.Id, owner.Name, owner.Email, apps);
    }

    private sealed record OwnerRow(string Id, string Name, string Email);
}
