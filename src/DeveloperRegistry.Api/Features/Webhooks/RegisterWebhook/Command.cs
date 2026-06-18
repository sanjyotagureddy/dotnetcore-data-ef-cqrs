namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public sealed record Command(string ApplicationId, string EventName, string Url);
