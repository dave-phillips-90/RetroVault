using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using RetroVault.Server.Models;
using RetroVault.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add configuration object for the database
builder.Services
    .AddOptions<CosmosDbOptions>()
    .Bind(builder.Configuration.GetSection(CosmosDbOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.EndpointUri), "EndpointUri is required. Set via appsettings.json or by setting the COSMOSDB__ENDPOINTURI environment variable")
    .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName), "DatabaseName is required.")
    .Validate(o => !(o.Emulator.Enabled && string.IsNullOrWhiteSpace(o.Emulator.AccountKey)), "AccountKey is required when using the emulator.")
    .ValidateOnStart();

// Create a Cosmos DB client singleton
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    CosmosDbOptions config = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
    CosmosClientOptions options = new()
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        },
        ConnectionMode = config.Emulator.Enabled ? ConnectionMode.Gateway : ConnectionMode.Direct,
        LimitToEndpoint = config.Emulator.Enabled
    };

    // If use emulator is true, connect to the endpoint with the account key
    if (config.Emulator.Enabled)
    {
        return new CosmosClient(config.EndpointUri, config.Emulator.AccountKey, options);
    }

    return new CosmosClient(config.EndpointUri, new DefaultAzureCredential(), options);
});

// If this is a development environment ensure that the database and container are created first
if (builder.Environment.IsDevelopment())
{
    // Adds a hosted service that creates the database and container asynchronously, guaranteed to complete before `app.Run()`
    builder.Services.AddHostedService<CosmosDbInitialiserHostedService>();
}

builder.Services.AddSingleton<CosmosDbService>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
