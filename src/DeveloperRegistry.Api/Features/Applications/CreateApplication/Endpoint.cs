using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Applications.CreateApplication;

public static class Endpoint
{
    public static RouteGroupBuilder MapCreateApplication(this RouteGroupBuilder group)
    {
        group.MapPost("/applications", HandleAsync)
            .WithName("CreateApplication")
            .WithSummary("Registers a new application");

        return group;
    }

    private static async Task<Results<Created<Response>, ProblemHttpResult>> HandleAsync(
        Command command,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(command, cancellationToken);
        return TypedResults.Created($"/api/v1/applications/{response.Id}", response);
    }
}
