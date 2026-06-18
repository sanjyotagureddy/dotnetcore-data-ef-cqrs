using System.Data;

namespace DeveloperRegistry.Api.Persistence;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken);
}