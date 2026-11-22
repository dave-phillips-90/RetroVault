using System.Text.Json.Serialization;

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
            [JsonPropertyName("displayName")]
            public required string DisplayName { get; set; }
            [JsonPropertyName("bio")]
            public string? Bio { get; set; }
            [JsonPropertyName("avatarUrl")]
            public string? AvatarUrl { get; set; }
        }
        public class UserSettings
        {
            public class NotificationSettings
            {
                [JsonPropertyName("emailNotifications")]
                public bool EmailNotifications { get; set; } = false;
                [JsonPropertyName("pushNotifications")]
                public bool PushNotifications { get; set; } = false;
            }
            [JsonPropertyName("theme")]
            public Themes Theme { get; set; } = Themes.System;
            [JsonPropertyName("notifications")]
            public NotificationSettings Notifications { get; set; } = new();
        }

        [JsonPropertyName("id")]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [JsonPropertyName("username")]
        public required string Username { get; set; }
        [JsonPropertyName("passwordHash")]
        public required string PasswordHash { get; set; }
        [JsonPropertyName("passwordSalt")]
        public required string PasswordSalt { get; set; }
        [JsonPropertyName("profile")]
        public required UserProfile Profile { get; set; }
        [JsonPropertyName("settings")]
        public required UserSettings Settings { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    }
}
