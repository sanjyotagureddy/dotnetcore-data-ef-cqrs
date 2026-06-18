namespace DeveloperRegistry.Api.Features.Applications.UpdateApplication;

public sealed record Command(string Id, string Name, string Description);
