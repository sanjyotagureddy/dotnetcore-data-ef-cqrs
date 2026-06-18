namespace DeveloperRegistry.Api.Features.Owners.AddOwner;

public sealed record Response(string OwnerId, string ApplicationId, string Name, string Email);
