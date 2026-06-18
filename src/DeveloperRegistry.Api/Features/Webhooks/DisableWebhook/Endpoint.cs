using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Webhooks.DisableWebhook;

public static class Endpoint
{
    public static RouteGroupBuilder MapDisableWebhook(this RouteGroupBuilder group)
    {
        group.MapPost("/webhooks/{id}/disable", HandleAsync)
            .WithName("DisableWebhook")
            .WithSummary("Disables a webhook");

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
