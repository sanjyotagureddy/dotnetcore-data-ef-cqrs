namespace DeveloperRegistry.Api.Common;

public static class IdGenerator
{
    public static string NewId() => NUlid.Ulid.NewUlid().ToString();
}
