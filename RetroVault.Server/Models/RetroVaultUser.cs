using Newtonsoft.Json;

namespace RetroVault.Server.Models
{
    public enum Themes
    {
        Light,
        Dark,
        System

    }
    
    public class RetroVaultUser
    {
        public class UserProfile
        {
            [JsonProperty("displayName")]
            public required string DisplayName { get; set; }
            [JsonProperty("bio")]
            public string? Bio { get; set; }
            [JsonProperty("avatarUrl")]
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
            [JsonProperty("theme")]
            public Themes Theme { get; set; } = Themes.System;
            [JsonProperty("notifications")]
            public NotificationSettings Notifications { get; set; } = new();
        }

        [JsonProperty("id")]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [JsonProperty("email")]
        public required string Email { get; set; }
        [JsonProperty("username")]
        public required string Username { get; set; }
        [JsonProperty("passwordHash")]
        public required string PasswordHash { get; set; }
        [JsonProperty("passwordSalt")]
        public required string PasswordSalt { get; set; }
        [JsonProperty("profile")]
        public required UserProfile Profile { get; set; }
        [JsonProperty("settings")]
        public required UserSettings Settings { get; set; }
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    }
}
