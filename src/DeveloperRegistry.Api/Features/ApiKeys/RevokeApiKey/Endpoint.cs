using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.ApiKeys.RevokeApiKey;

public static class Endpoint
{
    public static RouteGroupBuilder MapRevokeApiKey(this RouteGroupBuilder group)
    {
        group.MapPost("/apikeys/{id}/revoke", HandleAsync)
            .WithName("RevokeApiKey")
            .WithSummary("Revokes an API key");

        return group;
    }

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id), cancellationToken);
        return TypedResults.Ok(response);
    }
}
