namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public sealed record Response(string Id, string ApplicationId, string EventName, string Url, bool Enabled);
