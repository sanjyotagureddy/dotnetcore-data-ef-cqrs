using Microsoft.AspNetCore.Http.HttpResults;

namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public static class Endpoint
{
    public static RouteGroupBuilder MapRegisterWebhook(this RouteGroupBuilder group)
    {
        group.MapPost("/applications/{id}/webhooks", HandleAsync)
            .WithName("RegisterWebhook")
            .WithSummary("Registers a webhook for an application");

        return group;
    }

    private static async Task<Results<Created<Response>, ProblemHttpResult>> HandleAsync(
        string id,
        Request request,
        Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new Command(id, request.EventName, request.Url), cancellationToken);
        return TypedResults.Created($"/api/v1/webhooks/{response.Id}", response);
    }

    public sealed record Request(string EventName, string Url);
}
