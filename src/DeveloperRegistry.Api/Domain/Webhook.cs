namespace DeveloperRegistry.Api.Domain;

public sealed class Webhook
{
    private Webhook()
    {
    }

    public string Id { get; private set; } = string.Empty;

    public string ApplicationId { get; private set; } = string.Empty;

    public string EventName { get; private set; } = string.Empty;

    public string Url { get; private set; } = string.Empty;

    public bool Enabled { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public RegisteredApplication Application { get; private set; } = default!;

    public static Webhook Create(string id, string applicationId, string eventName, string url, DateTime utcNow)
    {
        return new Webhook
        {
            Id = id,
            ApplicationId = applicationId,
            EventName = eventName.Trim(),
            Url = url.Trim(),
            Enabled = true,
            CreatedAtUtc = utcNow,
        };
    }

    public void Enable() => Enabled = true;

    public void Disable() => Enabled = false;
}
