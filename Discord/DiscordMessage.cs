using System.Text.Json.Serialization;
#pragma warning disable 8618

namespace DiscordTokenStealer.Discord
{
    public class DiscordMessage
    {
        public DiscordMessage(string content, string? username = null, string? avatarUrl = null)
        {
            Content = content.Length >= 2000 ? content[..2001] : content;
            Username = username;
            AvatarUrl = avatarUrl;
        }

        public DiscordMessage()
        {
            
        }
        
        [JsonPropertyName("content")] public string Content { get; init; }
        [JsonPropertyName("username")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Username { get; init; }
        [JsonPropertyName("avatar_url")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? AvatarUrl { get; init; }
    }
}