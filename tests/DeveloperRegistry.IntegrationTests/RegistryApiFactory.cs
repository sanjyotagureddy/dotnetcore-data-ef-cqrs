using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace DeveloperRegistry.IntegrationTests;

public sealed class RegistryApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithDatabase("registrydb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithImage("postgres:17-alpine")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var values = new Dictionary<string, string?>
            {
                ["ConnectionStrings:registrydb"] = _postgres.GetConnectionString(),
            };
            configBuilder.AddInMemoryCollection(values);
        });
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await base.DisposeAsync();
    }
}
