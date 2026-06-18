using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Owners.AddOwner;

public static class Endpoint
{
    public static RouteGroupBuilder MapAddOwner(this RouteGroupBuilder group)
    {
        group.MapPost("/applications/{id}/owners", HandleAsync)
            .WithName("AddOwner")
            .WithSummary("Adds an owner to an application");

        return group;
    }

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Request request,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, request.Name, request.Email), cancellationToken);
        return TypedResults.Ok(response);
    }

    public sealed record Request(string Name, string Email);
}
