using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Core.Tests.Urls;

public class ShortUrlGeneratorScenarios
{
    [Fact]
    public void Should_return_short_url_for_zero()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(0, 10000);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("0");
    }

    [Fact]
    public void Should_return_short_url_for_1001()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(10001, 20000);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("2bJ");
    }
}