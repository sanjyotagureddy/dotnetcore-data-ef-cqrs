namespace DeveloperRegistry.Api.Domain;

public sealed class Owner
{
    private readonly List<ApplicationOwner> _applicationOwners = [];

    private Owner()
    {
    }

    public string Id { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<ApplicationOwner> ApplicationOwners => _applicationOwners;

    public static Owner Create(string id, string name, string email, DateTime utcNow)
    {
        return new Owner
        {
            Id = id,
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            CreatedAtUtc = utcNow,
        };
    }
}
