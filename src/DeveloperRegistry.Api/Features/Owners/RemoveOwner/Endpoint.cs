using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Owners.RemoveOwner;

public static class Endpoint
{
    public static RouteGroupBuilder MapRemoveOwner(this RouteGroupBuilder group)
    {
        group.MapDelete("/applications/{id}/owners/{ownerId}", HandleAsync)
            .WithName("RemoveOwner")
            .WithSummary("Removes an owner from an application");

        return group;
    }

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        string ownerId,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, ownerId), cancellationToken);
        return TypedResults.Ok(response);
    }
}
