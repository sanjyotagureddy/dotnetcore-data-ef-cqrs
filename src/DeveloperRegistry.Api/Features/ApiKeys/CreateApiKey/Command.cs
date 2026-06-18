namespace DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;

public sealed record Command(string ApplicationId, string Name, DateTime? ExpiresAtUtc);
