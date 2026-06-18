using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Webhooks.EnableWebhook;

public static class Endpoint
{
    public static RouteGroupBuilder MapEnableWebhook(this RouteGroupBuilder group)
    {
        group.MapPost("/webhooks/{id}/enable", HandleAsync)
            .WithName("EnableWebhook")
            .WithSummary("Enables a webhook");

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
