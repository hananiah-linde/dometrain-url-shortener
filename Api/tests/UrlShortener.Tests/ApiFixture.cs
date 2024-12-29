using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api;

namespace UrlShortener.Tests;

public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls("https://localhost:5001"); // Specify HTTPS URL

        builder.ConfigureServices(services =>
        {
            services.Configure<HttpsRedirectionOptions>(options =>
            {
                options.HttpsPort = 5001;
            });
        });
    }
}