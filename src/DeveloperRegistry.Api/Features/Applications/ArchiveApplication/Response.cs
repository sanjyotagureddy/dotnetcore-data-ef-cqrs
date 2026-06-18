namespace DeveloperRegistry.Api.Features.Applications.ArchiveApplication;

public sealed record Response(string Id, bool IsArchived, DateTime UpdatedAtUtc);
