public class TokenManager : IHostedService
{
    private readonly HttpClient _httpClient;

    public TokenManager(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TokenRangeService");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}