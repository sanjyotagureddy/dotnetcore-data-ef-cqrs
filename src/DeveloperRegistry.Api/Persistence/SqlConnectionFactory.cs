using System.Data;
using Npgsql;

namespace DeveloperRegistry.Api.Persistence;

public sealed class SqlConnectionFactory(NpgsqlDataSource dataSource) : ISqlConnectionFactory
{
    public async Task<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        return await dataSource.OpenConnectionAsync(cancellationToken);
    }
}
