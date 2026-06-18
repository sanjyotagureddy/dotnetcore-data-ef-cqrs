namespace DeveloperRegistry.Api.Features.Webhooks.GetApplicationWebhooks;

public sealed record Response(string Id, string EventName, string Url, bool Enabled);
