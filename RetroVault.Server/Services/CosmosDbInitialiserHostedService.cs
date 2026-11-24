using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using RetroVault.Server.Models;
using System.Net;
using System.Security.Authentication;

namespace RetroVault.Server.Services
{
    public class CosmosDbInitialiserHostedService(CosmosClient cosmosClient,
    IOptions<CosmosDbOptions> options,
    ILogger<CosmosDbInitialiserHostedService> logger) : IHostedLifecycleService
    {
        private readonly CosmosDbOptions _options = options.Value;
        public async Task StartingAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Ensuring Cosmos DB database '{DatabaseName}' and container '{ContainerName}' exist...",
                _options.DatabaseName, _options.ContainerName);

            try
            {
                DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(
                    _options.DatabaseName,
                    _options.Emulator.ThroughputRate,
                    cancellationToken: cancellationToken);

                logger.LogInformation("Database '{DatabaseName}' {Outcome}.",
                    _options.DatabaseName,
                    databaseResponse.StatusCode == HttpStatusCode.Created ? "created" : "already exists");

                ContainerResponse containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(
                    new ContainerProperties(_options.ContainerName, _options.PartitionKeyPath),
                    cancellationToken: cancellationToken);

                logger.LogInformation("Container '{ContainerName}' {Outcome}.",
                    _options.ContainerName,
                    containerResponse.StatusCode == HttpStatusCode.Created ? "created" : "already exists");
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidOperationException(
                    $"Cosmos DB rejected the configuration (database '{_options.DatabaseName}', " +
                    $"container '{_options.ContainerName}', partition key '{_options.PartitionKeyPath}', " +
                    $"throughput {_options.Emulator.ThroughputRate}).", ex);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidOperationException(
                    $"Cosmos DB at {_options.EndpointUri} rejected the configured account key.", ex);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new InvalidCredentialException(
                    $"Insufficient permissions to create or read the database at {_options.EndpointUri}.", ex);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
