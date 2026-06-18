namespace DeveloperRegistry.Api.Features.Owners.RemoveOwner;

public sealed record Response(string ApplicationId, string OwnerId, bool Removed);
