using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrlShortener.CosmosDbTriggerFunction;

public class HealthCheck
{
    private readonly HealthCheckService _healthCheckService;

    public HealthCheck(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [Function(nameof(HealthCheck))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "healthz")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var healthStatus = await _healthCheckService.CheckHealthAsync();
        return new OkObjectResult(
            Enum.GetName(typeof(HealthStatus), healthStatus.Status));
    }
}