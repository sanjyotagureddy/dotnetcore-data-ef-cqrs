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
        if (IsArchived)
        {
            throw new InvalidOperationException("Cannot update an archived application.");
        }

        Name = name.Trim();
        Description = description.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void Archive(DateTime utcNow)
    {
        if (IsArchived)
        {
            throw new InvalidOperationException("Application is already archived.");
        }

        IsArchived = true;
        UpdatedAtUtc = utcNow;
    }

    public ApiKey AddApiKey(string id, string name, string keyHash, DateTime utcNow, DateTime? expiresAtUtc)
    {
        if (IsArchived)
        {
            throw new InvalidOperationException("Cannot add an API key to an archived application.");
        }

        var apiKey = ApiKey.Create(id, Id, name, keyHash, utcNow, expiresAtUtc);
        _apiKeys.Add(apiKey);
        return apiKey;
    }

    public Webhook RegisterWebhook(string id, string eventName, string url, DateTime utcNow)
    {
        if (IsArchived)
        {
            throw new InvalidOperationException("Cannot register a webhook for an archived application.");
        }

        var webhook = Webhook.Create(id, Id, eventName, url, utcNow);
        _webhooks.Add(webhook);
        return webhook;
    }

    public ApplicationOwner AddOwner(string ownerId, DateTime utcNow)
    {
        if (IsArchived)
        {
            throw new InvalidOperationException("Cannot add an owner to an archived application.");
        }

        var link = ApplicationOwner.Create(Id, ownerId, utcNow);
        _applicationOwners.Add(link);
        return link;
    }
}
