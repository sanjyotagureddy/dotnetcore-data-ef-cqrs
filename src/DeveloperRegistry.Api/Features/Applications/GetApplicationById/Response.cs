namespace DeveloperRegistry.Api.Features.Applications.GetApplicationById;

public sealed record OwnerItem(string Id, string Name, string Email);

public sealed record ApiKeyItem(string Id, string Name, string Status, DateTime CreatedAtUtc, DateTime? ExpiresAtUtc);

public sealed record WebhookItem(string Id, string EventName, string Url, bool Enabled);

public sealed record ApplicationDetails(
    string Id,
    string Name,
    string Description,
    bool IsArchived,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    IReadOnlyList<OwnerItem> Owners,
    IReadOnlyList<ApiKeyItem> ApiKeys,
    IReadOnlyList<WebhookItem> Webhooks);
