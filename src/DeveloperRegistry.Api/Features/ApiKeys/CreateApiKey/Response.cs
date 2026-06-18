namespace DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;

public sealed record Response(string Id, string ApplicationId, string Name, string PlainTextKey, DateTime CreatedAtUtc, DateTime? ExpiresAtUtc);
