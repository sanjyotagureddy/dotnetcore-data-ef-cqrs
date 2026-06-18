using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Applications.UpdateApplication;

public static class Endpoint
{
    public static RouteGroupBuilder MapUpdateApplication(this RouteGroupBuilder group)
    {
        group.MapPut("/applications/{id}", HandleAsync)
            .WithName("UpdateApplication")
            .WithSummary("Updates application metadata");

        return group;
    }

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Request request,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, request.Name, request.Description), cancellationToken);
        return TypedResults.Ok(response);
    }

    public sealed record Request(string Name, string Description);
}
