using Microsoft.Azure.Cosmos;
using RetroVault.Server.Models;

namespace RetroVault.Server.Services
{
    public class CosmosService
    {
        private static readonly string EndpointUri = "AccountEndpoint=https://127.0.0.1:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public async Task<bool> AddUserAsync(RetroVaultUser user)
        {
            try
            {
                var options = new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Gateway,
                    LimitToEndpoint = true,
                };
                var cosmosClient = new CosmosClient(EndpointUri, options);

                string databaseName = "RetroVaultDB";
                string containerName = "Users";

                var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
                var database = databaseResponse.Database;

                ContainerProperties containerProperties = new(
                    id: containerName,
                    partitionKeyPath: "/id"
                );

                var containerResponse = await database.CreateContainerIfNotExistsAsync(containerProperties, ThroughputProperties.CreateManualThroughput(400));
                var container = containerResponse.Container;

                await container.CreateItemAsync(user, new PartitionKey(user.Id));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add user: {ex.Message}");
                return false;
            }
        }
    }
}
