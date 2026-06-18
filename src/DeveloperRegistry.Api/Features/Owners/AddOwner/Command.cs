namespace DeveloperRegistry.Api.Features.Owners.AddOwner;

public sealed record Command(string ApplicationId, string Name, string Email);
