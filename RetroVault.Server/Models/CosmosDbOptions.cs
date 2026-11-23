namespace RetroVault.Server.Models
{
    public sealed class CosmosDbOptions
    {
        public const string SectionName = "CosmosDB";

        public string EndpointUri { get; init; } = string.Empty; // The endpoint URI to connect to the Cosmos Database
        public string DatabaseName { get; init; } = "RetroVaultDB"; // The database name to access
        public string ContainerName { get; init; } = "Users"; // The container name to access
        public string PartitionKeyPath { get; init; } = "/pk"; // The partition key path of the container
        public bool UseEmulator { get; set; } = false; // Whether to use the local emulator or the Azure hosted instance
        public string? AccountKey { get; init; } // The account key, required when using the local emulator as `DefaultAzureCredential()` does not apply
    }
}
