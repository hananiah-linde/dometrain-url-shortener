using System.Net.Http.Json;
using FluentAssertions;

namespace UrlShortener.TokenRangeService.Tests;

public class AssignTokenRangeScenarios : IClassFixture<Fixture>
{
    private readonly HttpClient _client;

    public AssignTokenRangeScenarios(Fixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Should_return_range_when_requested()
    {
        var requestResponse = await _client.PostAsJsonAsync("/assign",
            new
            {
                Key = "tests"
            });

        var tokenRange = await requestResponse.Content.ReadFromJsonAsync<TokenRangeResponse>();

        tokenRange.Start.Should().BeGreaterThan(0);
        tokenRange.End.Should().BeGreaterThan(tokenRange.Start);
    }
}

public record TokenRangeResponse(long Start, long End);