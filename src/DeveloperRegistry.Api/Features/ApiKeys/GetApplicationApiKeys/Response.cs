namespace DeveloperRegistry.Api.Features.ApiKeys.GetApplicationApiKeys;

public sealed record Response(string Id, string Name, string Status, DateTime CreatedAtUtc, DateTime? ExpiresAtUtc);
