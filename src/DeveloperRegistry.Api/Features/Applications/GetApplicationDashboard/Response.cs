namespace DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard;

public sealed record ApplicationDashboard(
    string Id,
    string Name,
    bool IsArchived,
    int OwnerCount,
    int ActiveApiKeyCount,
    int WebhookCount,
    int EnabledWebhookCount,
    DateTime UpdatedAtUtc);
