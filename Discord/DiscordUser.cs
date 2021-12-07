using System.Text;
using System.Text.Json.Serialization;

namespace DiscordTokenStealer.Discord;

public class DiscordUser
{
    private const string Empty = "None";
    [JsonPropertyName("username")] [JsonInclude] public string Username { get; private set; }
    [JsonPropertyName("discriminator")] [JsonInclude] public string Discriminator { get; private set; }
    [JsonPropertyName("id")] [JsonInclude] public string Id { get; private set; }
    [JsonPropertyName("locale")] [JsonInclude] public string Locale { get; private set; }
    [JsonPropertyName("mfa_enabled")] [JsonInclude] public bool TwoFactor { get; private set; }
    [JsonPropertyName("email")] [JsonInclude] public string Email { get; private set; }
    [JsonPropertyName("verified")] [JsonInclude] public bool EmailVerified { get; private set; }
    [JsonPropertyName("premium_type")] [JsonInclude] public int PremiumType { get; private set; } = 0;

    [JsonIgnore] private string? _phoneNumber;
    [JsonPropertyName("phone")] [JsonInclude] public string PhoneNumber
    {
        get
        {
            if (string.IsNullOrEmpty(_phoneNumber))
            {
                return Empty;
            }
            return _phoneNumber;
        }
        private set => _phoneNumber = value;
    }

    [JsonIgnore] private string? _aboutMe;
    [JsonPropertyName("bio")] [JsonInclude] public string AboutMe
    {
        get
        {
            if (string.IsNullOrEmpty(_aboutMe))
            {
                return Empty;
            }
            return _aboutMe;
        }
        private set => _aboutMe = value;
    }

    public override string ToString()
    {
        
        return new StringBuilder()
                .AppendLine($"\tSummary:")
                .AppendLine($"\t\tUser: {Username}#{Discriminator} ({Id})")
                .AppendLine($"\t\tEmail: {Email}")
                .AppendLine($"\t\tPhone: {PhoneNumber}")
                .AppendLine($"\t\tLocale: {Locale}")
                .AppendLine($"\t\tVerified: {EmailVerified}")
                .AppendLine($"\t\tTwo-Factor: {TwoFactor}")
                .AppendLine($"\t\tAbout Me: {AboutMe}")
                .AppendLine($"\t\tNitro: {(DiscordNitroType)PremiumType}")
                .ToString();
    }
}