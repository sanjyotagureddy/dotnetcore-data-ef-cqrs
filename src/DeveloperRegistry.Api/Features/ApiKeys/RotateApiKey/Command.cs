namespace DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;

public sealed record Command(string Id, DateTime? ExpiresAtUtc);
