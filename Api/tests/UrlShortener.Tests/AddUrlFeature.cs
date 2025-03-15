﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.Core.Urls.Add;
using FluentAssertions;

namespace UrlShortener.Tests;

public class AddUrlFeature : IClassFixture<ApiFixture>
{
    private readonly HttpClient _client;

    public AddUrlFeature(ApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme: "TestScheme");
    }
    [Fact]
    public async Task Given_long_url_Should_return_short_url()
    {
        var response = await _client.PostAsJsonAsync<AddUrlRequest>("/api/urls",
            new AddUrlRequest(new Uri("https://www.dometrain.com"), string.Empty));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var addUrlResponse = await response.Content.ReadFromJsonAsync<AddUrlResponse>();
        addUrlResponse.Should().NotBeNull();
        addUrlResponse!.ShortUrl.Should().NotBeNullOrEmpty();
    }
}