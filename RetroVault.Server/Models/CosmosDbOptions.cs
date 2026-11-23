namespace RetroVault.Server.Models
{
    public sealed class CosmosDbEmulatorOptions
    {
        public bool Enabled { get; init; } = false;
        public string? AccountKey { get; init; }
        public int ThroughputRate { get; init; } = 400;
    }

    public sealed class CosmosDbOptions
    {
        public const string SectionName = "CosmosDB";

        public string EndpointUri { get; init; } = string.Empty; // The endpoint URI to connect to the Cosmos Database
        public string DatabaseName { get; init; } = "RetroVaultDB"; // The database name to access
        public string ContainerName { get; init; } = "Users"; // The container name to access
        public string PartitionKeyPath { get; init; } = "/pk"; // The partition key path of the container
        public CosmosDbEmulatorOptions Emulator { get; init; } = new();
    }
}
