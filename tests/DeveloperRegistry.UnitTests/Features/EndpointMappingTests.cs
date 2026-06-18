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
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DeveloperRegistry.UnitTests.Features;

public sealed class EndpointMappingTests
{
    public static TheoryData<Func<RouteGroupBuilder, RouteGroupBuilder>> Mappers =>
    [
        group => DeveloperRegistry.Api.Features.Applications.CreateApplication.Endpoint.MapCreateApplication(group),
        group => DeveloperRegistry.Api.Features.Applications.UpdateApplication.Endpoint.MapUpdateApplication(group),
        group => DeveloperRegistry.Api.Features.Applications.ArchiveApplication.Endpoint.MapArchiveApplication(group),
        group => DeveloperRegistry.Api.Features.Owners.AddOwner.Endpoint.MapAddOwner(group),
        group => DeveloperRegistry.Api.Features.Owners.RemoveOwner.Endpoint.MapRemoveOwner(group),
        group => DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey.Endpoint.MapCreateApiKey(group),
        group => DeveloperRegistry.Api.Features.ApiKeys.RevokeApiKey.Endpoint.MapRevokeApiKey(group),
        group => DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey.Endpoint.MapRotateApiKey(group),
        group => DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook.Endpoint.MapRegisterWebhook(group),
        group => DeveloperRegistry.Api.Features.Webhooks.EnableWebhook.Endpoint.MapEnableWebhook(group),
        group => DeveloperRegistry.Api.Features.Webhooks.DisableWebhook.Endpoint.MapDisableWebhook(group),
    ];

    [Theory]
    [MemberData(nameof(Mappers))]
    public void MapEndpoint_ShouldRegisterRoute_AndReturnSameGroup(Func<RouteGroupBuilder, RouteGroupBuilder> map)
    {
        var builder = WebApplication.CreateBuilder();
        using var app = builder.Build();
        var group = app.MapGroup("/api/v1");

        var returnedGroup = map(group);

        returnedGroup.Should().BeSameAs(group);
    }
}
