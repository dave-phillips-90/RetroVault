using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RetroVault.Server.Models
{
    public enum Themes
    {
        Light,
        Dark,
        System

    }
    
    public class RetroVaultUser : CosmosDocument
    {
        public const string DocumentType = "user";
        public class UserProfile
        {
            public required string DisplayName { get; set; }
            public string? Bio { get; set; }
            public string? AvatarUrl { get; set; }
        }
        public class UserSettings
        {
            public class NotificationSettings
            {
                [JsonProperty("emailNotifications")]
                public bool EmailNotifications { get; set; } = false;
                [JsonProperty("pushNotifications")]
                public bool PushNotifications { get; set; } = false;
            }
            [JsonConverter(typeof(StringEnumConverter))]
            public Themes Theme { get; set; } = Themes.System;
            public NotificationSettings Notifications { get; set; } = new();
        }
        public override string Type => DocumentType;
        public override string Pk => Id; // Partition key is the same as the document ID for the user
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string HashedPassword { get; set; }
        public required UserProfile Profile { get; set; }
        public required UserSettings Settings { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    }
}
