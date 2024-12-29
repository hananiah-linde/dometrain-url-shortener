using UrlShortener.Core;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlFeature(this IServiceCollection services)
    {
        services.AddScoped<AddUrlHandler>();
        services.AddScoped<ShortUrlGenerator>();

        services.AddSingleton<TokenProvider>(_ =>
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(0, 10000);
            return tokenProvider;
        });
        services.AddSingleton<IUrlDataStore, InMemoryUrlDataStore>();

        return services;
    }
}

public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken)
    {
        Add(shortenedUrl.ShortUrl, shortenedUrl);
        return Task.CompletedTask;
    }
}