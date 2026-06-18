using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;

public static class Endpoint
{
    public static RouteGroupBuilder MapCreateApiKey(this RouteGroupBuilder group)
    {
        group.MapPost("/applications/{id}/apikeys", HandleAsync)
            .WithName("CreateApiKey")
            .WithSummary("Creates a new API key for an application");

        return group;
    }

    private static async Task<Results<Created<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Request request,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, request.Name, request.ExpiresAtUtc), cancellationToken);
        return TypedResults.Created($"/api/v1/apikeys/{response.Id}", response);
    }

    public sealed record Request(string Name, DateTime? ExpiresAtUtc);
}
