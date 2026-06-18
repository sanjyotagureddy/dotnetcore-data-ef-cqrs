namespace DeveloperRegistry.Api.Domain;

public sealed class ApplicationOwner
{
    private ApplicationOwner()
    {
    }

    public string ApplicationId { get; private set; } = string.Empty;

    public string OwnerId { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public RegisteredApplication Application { get; private set; } = default!;

    public Owner Owner { get; private set; } = default!;

    public static ApplicationOwner Create(string applicationId, string ownerId, DateTime utcNow)
    {
        return new ApplicationOwner
        {
            ApplicationId = applicationId,
            OwnerId = ownerId,
            CreatedAtUtc = utcNow,
        };
    }
}
