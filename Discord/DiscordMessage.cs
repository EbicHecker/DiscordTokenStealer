using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordMessage
    {
        protected DiscordMessage(string content, string? username = null, string? avatarUrl = null)
        {
            Content = content.Length >= 2000 ? content[..2001] : content;
            Username = username;
            AvatarUrl = avatarUrl;
        }

        public DiscordMessage()
        {

        }

        [JsonPropertyName("content")] public string Content { get; init; } = null!;
        [JsonPropertyName("username")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Username { get; init; }
        [JsonPropertyName("avatar_url")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? AvatarUrl { get; init; }
    }

    public class HelloWorld : DiscordMessage
    {
        public HelloWorld() : base("Hello, World!")
        {

        }
    }
}