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

    [JsonPropertyName("content")] public string Content { get; }

    [JsonPropertyName("username")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Username { get; }

    [JsonPropertyName("avatar_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AvatarUrl { get; }
}