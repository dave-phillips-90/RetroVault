using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using RetroVault.Server.Models;

namespace RetroVault.Server.Services
{
    public class CosmosDbInitialiserHostedService(CosmosClient cosmosClient,
    IOptions<CosmosDbOptions> options,
    ILogger<CosmosDbInitialiserHostedService> logger) : IHostedService
    {
        private readonly CosmosDbOptions _options = options.Value;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Ensuring Cosmos DB database '{DatabaseName}' and container '{ContainerName}' exist...",
            _options.DatabaseName,
            _options.ContainerName);

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
                _options.DatabaseName,
                ThroughputProperties.CreateManualThroughput(400),
                cancellationToken: cancellationToken);

            ContainerProperties containerProperties = new(
                id: _options.ContainerName,
                partitionKeyPath: _options.PartitionKeyPath
            );

            await database.CreateContainerIfNotExistsAsync(
                containerProperties,
                cancellationToken: cancellationToken);

            logger.LogInformation("Cosmos DB initialization complete.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
