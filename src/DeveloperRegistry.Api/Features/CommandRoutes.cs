using DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;
using DeveloperRegistry.Api.Features.ApiKeys.RevokeApiKey;
using DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;
using DeveloperRegistry.Api.Features.Applications.ArchiveApplication;
using DeveloperRegistry.Api.Features.Applications.CreateApplication;
using DeveloperRegistry.Api.Features.Applications.UpdateApplication;
using DeveloperRegistry.Api.Features.Owners.AddOwner;
using DeveloperRegistry.Api.Features.Owners.RemoveOwner;
using DeveloperRegistry.Api.Features.Webhooks.DisableWebhook;
using DeveloperRegistry.Api.Features.Webhooks.EnableWebhook;
using DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

namespace DeveloperRegistry.Api.Features;

public static class CommandRoutes
{
    public static RouteGroupBuilder MapCommandEndpoints(this RouteGroupBuilder group)
    {
        group.MapCreateApplication();
        group.MapUpdateApplication();
        group.MapArchiveApplication();

        group.MapAddOwner();
        group.MapRemoveOwner();

        group.MapCreateApiKey();
        group.MapRevokeApiKey();
        group.MapRotateApiKey();

        group.MapRegisterWebhook();
        group.MapEnableWebhook();
        group.MapDisableWebhook();

        return group;
    }
}
