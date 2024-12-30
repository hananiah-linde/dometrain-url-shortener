using Microsoft.Azure.Cosmos;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Infrastructure;

public class CosmosDbUrlDataStore : IUrlDataStore
{
    private readonly Container _container;

    public CosmosDbUrlDataStore(Container container)
    {
        _container = container;
    }

    public async Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken)
    {
        await _container.CreateItemAsync(shortenedUrl,
            new PartitionKey(shortenedUrl.ShortUrl[..1]),
            cancellationToken: cancellationToken);
    }
}