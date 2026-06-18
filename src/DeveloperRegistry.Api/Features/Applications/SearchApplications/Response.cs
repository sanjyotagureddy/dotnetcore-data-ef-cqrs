namespace DeveloperRegistry.Api.Features.Applications.SearchApplications;

public sealed record ApplicationSearchItem(string Id, string Name, string Description, bool IsArchived, DateTime UpdatedAtUtc);
