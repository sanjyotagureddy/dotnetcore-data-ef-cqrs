using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Applications.ArchiveApplication;

public static class Endpoint
{
    public static RouteGroupBuilder MapArchiveApplication(this RouteGroupBuilder group)
    {
        group.MapDelete("/applications/{id}", HandleAsync)
            .WithName("ArchiveApplication")
            .WithSummary("Archives an application");

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
