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
            new AssignTokenRangeRequest("tests"));

        var tokenRange = await requestResponse.Content.ReadFromJsonAsync<TokenRangeResponse>();

        tokenRange.Start.Should().BeGreaterThan(0);
        tokenRange.End.Should().BeGreaterThan(tokenRange.Start);
    }

    [Fact]
    public async Task Should_not_repeat_range_when_requested()
    {
        var requestResponse1 = await _client.PostAsJsonAsync("/assign", new AssignTokenRangeRequest("tests"));
        var tokenRange1 = await requestResponse1.Content
            .ReadFromJsonAsync<TokenRangeResponse>();

        var requestResponse2 = await _client.PostAsJsonAsync("/assign", new AssignTokenRangeRequest("tests"));
        var tokenRange2 = await requestResponse2.Content
            .ReadFromJsonAsync<TokenRangeResponse>();

        tokenRange2.Start.Should().BeGreaterThan(tokenRange1.End);
    }
}