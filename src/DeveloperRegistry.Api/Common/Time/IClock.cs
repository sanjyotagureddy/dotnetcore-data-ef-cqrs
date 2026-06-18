namespace DeveloperRegistry.Api.Common.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}
