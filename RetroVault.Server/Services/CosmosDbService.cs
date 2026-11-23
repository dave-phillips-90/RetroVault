using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using RetroVault.Server.Models;

namespace RetroVault.Server.Services
{
    public class CosmosDbService(IOptions<CosmosDbOptions> options, CosmosClient cosmosClient, ILogger<CosmosDbService> logger)
    {
        private readonly Container _container = cosmosClient.GetDatabase(options.Value.DatabaseName)
            .GetContainer(options.Value.ContainerName);

        public async Task<bool> AddUserAsync(RetroVaultUser user)
        {
            try
            {
                await _container.CreateItemAsync(user, new PartitionKey(user.Pk));

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to add user: {Message}", ex.Message);
                return false;
            }
        }
    }
}
