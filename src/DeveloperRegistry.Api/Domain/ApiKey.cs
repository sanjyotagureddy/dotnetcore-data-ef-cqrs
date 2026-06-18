namespace DeveloperRegistry.Api.Domain;

public sealed class ApiKey
{
    private ApiKey()
    {
    }

    public string Id { get; private set; } = string.Empty;

    public string ApplicationId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string KeyHash { get; private set; } = string.Empty;

    public ApiKeyStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? ExpiresAtUtc { get; private set; }

    public RegisteredApplication Application { get; private set; } = default!;

    public static ApiKey Create(string id, string applicationId, string name, string keyHash, DateTime utcNow, DateTime? expiresAtUtc)
    {
        return new ApiKey
        {
            Id = id,
            ApplicationId = applicationId,
            Name = name.Trim(),
            KeyHash = keyHash,
            Status = ApiKeyStatus.Active,
            CreatedAtUtc = utcNow,
            ExpiresAtUtc = expiresAtUtc,
        };
    }

    public void Revoke()
    {
        if (Status == ApiKeyStatus.Revoked)
        {
            throw new InvalidOperationException("API key is already revoked.");
        }

        Status = ApiKeyStatus.Revoked;
    }

    public void Rotate(string keyHash, DateTime? expiresAtUtc)
    {
        if (Status == ApiKeyStatus.Revoked)
        {
            throw new InvalidOperationException("Cannot rotate a revoked API key.");
        }

        KeyHash = keyHash;
        ExpiresAtUtc = expiresAtUtc;
        Status = ApiKeyStatus.Active;
    }
}
