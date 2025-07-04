using Microsoft.Extensions.Logging;
using NSubstitute;
using StackExchange.Redis;
using UrlShortener.RedirectApi.Infrastructure;

namespace UrlShortener.RedirectApi.Tests;

[Collection("Api collection")]
public class RedisCacheScenarios
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheScenarios(ApiFixture fixture)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(fixture.RedisConnectionString);
    }

    [Fact]
    public async Task Should_get_from_reader_if_not_in_cache()
    {
        var reader = Substitute.For<IShortenedUrlReader>();
        var logger = Substitute.For<ILogger<RedisUrlReader>>();
        reader.GetLongUrlAsync("short", Arg.Any<CancellationToken>())
            .Returns(new ReadLongUrlResponse(true, "http://google.com"));
        var cache = new RedisUrlReader(reader, _connectionMultiplexer, logger);

        _ = await cache.GetLongUrlAsync("short", CancellationToken.None);

        await reader.Received().GetLongUrlAsync("short", Arg.Any<CancellationToken>());
    }
}