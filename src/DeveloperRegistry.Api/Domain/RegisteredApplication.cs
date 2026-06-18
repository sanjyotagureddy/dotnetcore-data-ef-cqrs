namespace DeveloperRegistry.Api.Domain;

public sealed class RegisteredApplication
{
    private readonly List<ApplicationOwner> _applicationOwners = [];
    private readonly List<ApiKey> _apiKeys = [];
    private readonly List<Webhook> _webhooks = [];

    private RegisteredApplication()
    {
    }

    public string Id { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool IsArchived { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public IReadOnlyCollection<ApplicationOwner> ApplicationOwners => _applicationOwners;

    public IReadOnlyCollection<ApiKey> ApiKeys => _apiKeys;

    public IReadOnlyCollection<Webhook> Webhooks => _webhooks;

    public static RegisteredApplication Create(string id, string name, string description, DateTime utcNow)
    {
        return new RegisteredApplication
        {
            Id = id,
            Name = name.Trim(),
            Description = description.Trim(),
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow,
        };
    }

    public void Update(string name, string description, DateTime utcNow)
    {
        Name = name.Trim();
        Description = description.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void Archive(DateTime utcNow)
    {
        IsArchived = true;
        UpdatedAtUtc = utcNow;
    }
}
