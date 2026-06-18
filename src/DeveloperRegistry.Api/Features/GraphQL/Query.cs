using HotChocolate;

namespace DeveloperRegistry.Api.Features.GraphQL;

public sealed class Query
{
    public async Task<DeveloperRegistry.Api.Features.Applications.GetApplicationById.ApplicationDetails?> ApplicationAsync(
        [ID] string id,
        DeveloperRegistry.Api.Features.Applications.GetApplicationById.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new DeveloperRegistry.Api.Features.Applications.GetApplicationById.Query(id), cancellationToken);
    }

    public async Task<IReadOnlyList<DeveloperRegistry.Api.Features.Applications.SearchApplications.ApplicationSearchItem>> ApplicationsAsync(
        string? search,
        DeveloperRegistry.Api.Features.Applications.SearchApplications.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new DeveloperRegistry.Api.Features.Applications.SearchApplications.Query(search), cancellationToken);
    }

    public async Task<DeveloperRegistry.Api.Features.Owners.GetOwnerApplications.OwnerDetails?> OwnerAsync(
        [ID] string id,
        DeveloperRegistry.Api.Features.Owners.GetOwnerApplications.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new DeveloperRegistry.Api.Features.Owners.GetOwnerApplications.Query(id), cancellationToken);
    }

    public async Task<DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.ApplicationDashboard?> ApplicationDashboardAsync(
        [ID] string id,
        DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard.Query(id), cancellationToken);
    }
}
