using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;

public static class Endpoint
{
    public static RouteGroupBuilder MapRotateApiKey(this RouteGroupBuilder group)
    {
        group.MapPost("/apikeys/{id}/rotate", HandleAsync)
            .WithName("RotateApiKey")
            .WithSummary("Rotates an API key");

        return group;
    }

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Request request,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, request.ExpiresAtUtc), cancellationToken);
        return TypedResults.Ok(response);
    }

    public sealed record Request(DateTime? ExpiresAtUtc);
}
