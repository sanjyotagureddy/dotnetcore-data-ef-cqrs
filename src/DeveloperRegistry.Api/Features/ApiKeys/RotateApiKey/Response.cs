namespace DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;

public sealed record Response(string Id, string PlainTextKey, string Status, DateTime? ExpiresAtUtc);
