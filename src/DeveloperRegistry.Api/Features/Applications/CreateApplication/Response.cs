namespace DeveloperRegistry.Api.Features.Applications.CreateApplication;

public sealed record Response(string Id, string Name, string Description, DateTime CreatedAtUtc);
