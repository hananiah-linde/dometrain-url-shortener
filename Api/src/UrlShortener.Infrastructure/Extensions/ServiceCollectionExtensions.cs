using Microsoft.Azure.Cosmos;
using UrlShortener.Core.Urls.Add;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosUrlDataStore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<CosmosClient>(s =>
            new CosmosClient(configuration["CosmosDb:ConnectionString"]!,
                new CosmosClientOptions { ConnectionMode = ConnectionMode.Gateway }));

        services.AddSingleton<IUrlDataStore>(s =>
        {
            var cosmosClient = s.GetRequiredService<CosmosClient>();

            var container =
                cosmosClient.GetContainer(
                    configuration["DatabaseName"]!,
                    configuration["ContainerName"]!);

            return new CosmosDbUrlDataStore(container);
        });

        return services;
    }
}