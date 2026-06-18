namespace DeveloperRegistry.Api.Features.Owners.RemoveOwner;

public sealed record Command(string ApplicationId, string OwnerId);
