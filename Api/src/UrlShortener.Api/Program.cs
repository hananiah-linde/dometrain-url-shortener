using Azure.Identity;
using UrlShortener.Api;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddSingleton(TimeProvider.System);
builder.Services
    .AddUrlFeature()
    .AddCosmosUrlDataStore(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Url Shortener API");

app.MapPost("/api/urls",
    async (AddUrlHandler handler,
        AddUrlRequest request,
        CancellationToken cancellationToken) =>
    {
        var requestWithUser = request with
        {
            CreatedBy = "me@example.com"
        };
        var result = await handler.HandleAsync(requestWithUser, cancellationToken);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created("/api/urls", result.Value!);

    });

app.Run();
