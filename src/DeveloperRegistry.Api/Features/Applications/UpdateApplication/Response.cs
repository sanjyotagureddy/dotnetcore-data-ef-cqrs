namespace DeveloperRegistry.Api.Features.Applications.UpdateApplication;

public sealed record Response(string Id, string Name, string Description, DateTime UpdatedAtUtc);
