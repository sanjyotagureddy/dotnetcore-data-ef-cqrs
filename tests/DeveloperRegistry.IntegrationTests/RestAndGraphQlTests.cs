using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace DeveloperRegistry.IntegrationTests;

public sealed class RestAndGraphQlTests : IClassFixture<RegistryApiFactory>
{
    private readonly HttpClient _client;

    public RestAndGraphQlTests(RegistryApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateApplication_ThenQueryByGraphQl_ShouldReturnApplication()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/applications", new
        {
            Name = "Portal",
            Description = "Developer portal",
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        using var createdDoc = JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        var appId = createdDoc.RootElement.GetProperty("id").GetString();
        appId.Should().NotBeNullOrWhiteSpace();

        var graphQlResponse = await _client.PostAsJsonAsync("/graphql", new
        {
            query = "query ($id: String!) { application(id: $id) { id name description } }",
            variables = new { id = appId },
        });

        graphQlResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var graphDoc = JsonDocument.Parse(await graphQlResponse.Content.ReadAsStringAsync());
        var returnedId = graphDoc.RootElement.GetProperty("data").GetProperty("application").GetProperty("id").GetString();
        returnedId.Should().Be(appId);
    }

    [Fact]
    public async Task HealthReady_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health/ready");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
