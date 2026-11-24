namespace RetroVault.Server.Models
{
    public abstract class CosmosDocument
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public abstract string Pk { get; }
        public abstract string Type { get; }
        public int SchemaVersion { get; set; } = 1;
    }
}
