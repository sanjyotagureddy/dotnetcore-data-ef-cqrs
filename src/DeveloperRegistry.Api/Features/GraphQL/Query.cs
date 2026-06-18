using DeveloperRegistry.Api.Features.Applications.GetApplicationById;
using DeveloperRegistry.Api.Features.Applications.GetApplicationDashboard;
using DeveloperRegistry.Api.Features.Applications.SearchApplications;
using DeveloperRegistry.Api.Features.Owners.GetOwnerApplications;
using HotChocolate;

namespace DeveloperRegistry.Api.Features.GraphQL;

public sealed class Query
{
    public async Task<ApplicationDetails?> ApplicationAsync(
        [ID] string id,
        Applications.GetApplicationById.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new Applications.GetApplicationById.Query(id), cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationSearchItem>> ApplicationsAsync(
        string? search,
        Applications.SearchApplications.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new Applications.SearchApplications.Query(search), cancellationToken);
    }

    public async Task<OwnerDetails?> OwnerAsync(
        [ID] string id,
        Owners.GetOwnerApplications.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new Owners.GetOwnerApplications.Query(id), cancellationToken);
    }

    public async Task<ApplicationDashboard?> ApplicationDashboardAsync(
        [ID] string id,
        Applications.GetApplicationDashboard.Handler handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(new Applications.GetApplicationDashboard.Query(id), cancellationToken);
    }
}
