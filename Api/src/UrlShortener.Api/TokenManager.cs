using UrlShortener.Core;

public class TokenManager : IHostedService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenManager> _logger;
    private TokenProvider _tokenProvider;

    private readonly string _machineIdentifier;

    public TokenManager(IHttpClientFactory httpClientFactory, ILogger<TokenManager> logger, TokenProvider tokenProvider)
    {
        _logger = logger;
        _tokenProvider = tokenProvider;
        _httpClient = httpClientFactory.CreateClient("TokenRangeService");
        _machineIdentifier = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? "unknown-machine-id";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting token manager");

        var response = await _httpClient.PostAsJsonAsync("assign",
            new { Key = _machineIdentifier }, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to assign new token range");
        }

        var range = await response.Content.ReadFromJsonAsync<TokenRange>(cancellationToken);

        if (range is null)
        {
            throw new Exception("No tokens assigned");
        }

        _tokenProvider.AssignRange(range);

        _logger.LogInformation("Token range assigned: {Start}-{End}", range.Start, range.End);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping token manager");
        return Task.CompletedTask;
    }
}