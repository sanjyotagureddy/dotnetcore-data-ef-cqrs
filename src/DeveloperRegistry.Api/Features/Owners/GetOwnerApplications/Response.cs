namespace DeveloperRegistry.Api.Features.Owners.GetOwnerApplications;

public sealed record ApplicationItem(string Id, string Name, string Description, bool IsArchived);

public sealed record OwnerDetails(string Id, string Name, string Email, IReadOnlyList<ApplicationItem> Applications);
