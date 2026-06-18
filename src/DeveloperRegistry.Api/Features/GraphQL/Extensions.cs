namespace DeveloperRegistry.Api.Features.GraphQL;

public static class Extensions
{
    public static IServiceCollection AddGraphQlQueries(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<Query>();

        return services;
    }
}
